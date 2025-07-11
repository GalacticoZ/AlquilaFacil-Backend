<script setup>
import NavbarComponent from '../../public/components/Navbar.component.vue';
import {ref, computed, onMounted} from 'vue';
import {RouterLink, useRoute} from 'vue-router';
import {LocalsApiService} from '../services/locals-api.service';
import {LocalResponse} from '../model/local.response';
import {ReservationRequest} from '../../booking/models/reservation.request';
import {useAuthenticationStore} from '../../auth/services/authentication.store';
import {ReservationsApiService} from '../../booking/services/reservations-api.service';
import {ProfilesApiService} from '../../profile/services/profiles-api.service';
import FooterComponent from '../../public/components/Footer.component.vue';
import {cloudinaryWidget} from '../../shared/components/cloudinary-widget';

const route = useRoute();
const authenticationStore = useAuthenticationStore();

const localsApiService = new LocalsApiService();
const reservationsApiService = new ReservationsApiService();
const profilesApiService = new ProfilesApiService();

const local = ref({});
const bankAccounts = ref({});
const startDate = ref('');
const endDate = ref('');
const selectedPhoto = ref('');
const voucherImageUrl = ref('');
const isLoaded = ref(false);

onMounted(async () => {
  const id = parseInt(route.params.id);
  const response = await localsApiService.getById(id);
  local.value = new LocalResponse(response);
  console.log("Local cargado:", local.value);
  bankAccounts.value = await profilesApiService.getBankAccountsByUserId(local.value.userId);
  selectedPhoto.value = local.value.photoUrls[0];
  isLoaded.value = true;
});

const now = () => new Date().toISOString().slice(0, 16);

const isStartDateValid = computed(() => {
  return startDate.value && startDate.value >= now();
});

const isEndDateValid = computed(() => {
  return (
      endDate.value &&
      startDate.value &&
      endDate.value > startDate.value
  );
});

const isFormValid = computed(() => isStartDateValid.value && isEndDateValid.value);

const totalAmountToPay = computed(() => {
  if (isFormValid.value) {
    const start = new Date(startDate.value);
    const end = new Date(endDate.value);
    const diffInMs = end.getTime() - start.getTime();
    const diffInHours = diffInMs / (1000 * 60 * 60);
    return Math.round(diffInHours * local.value.price * 100) / 100; // Redondear a 2 decimales
  }
  return 0.00;
});

const openUploadWidget = async () => {
  try {
    const secureUrl = await cloudinaryWidget();
    console.log("URL segura:", secureUrl);
    voucherImageUrl.value = secureUrl[0]
  } catch (error) {
    console.error("Error al subir imagen:", error);
  }
};

const reserveLocal = async () => {
  try {
    const localStartDate = new Date(startDate.value);
    const localEndDate = new Date(endDate.value);

    // Ajustar las fechas a UTC pero sin cambiar la hora (eliminando la zona horaria local)
    const startDateUTC = new Date(localStartDate.getTime() - localStartDate.getTimezoneOffset() * 60000);
    const endDateUTC = new Date(localEndDate.getTime() - localEndDate.getTimezoneOffset() * 60000);

    // Convertir las fechas a formato ISO, pero en UTC sin el ajuste de zona horaria (sin añadir 5 horas)
    const formattedStartDate = startDateUTC.toISOString();
    const formattedEndDate = endDateUTC.toISOString();

    // Crear la solicitud de reserva con las fechas formateadas
    const reservationRequest = new ReservationRequest({
      startDate: formattedStartDate,
      endDate: formattedEndDate,
      localId: local.value.id,
      userId: authenticationStore.userId,
      price: totalAmountToPay.value,
      voucherImageUrl: voucherImageUrl.value,
    });
    console.log(reservationRequest);
    await reservationsApiService.create(reservationRequest);
    alert('Reserva realizada correctamente');
  } catch (error) {
    console.error('Error al reservar el local:', error);
    alert('Error al reservar el local. Por favor, inténtelo de nuevo más tarde.');
  }
};
</script>

<template>
  <NavbarComponent/>
  <main class="px-4 sm:px-8 md:px-10 lg:px-16 py-10 w-full min-h-[80dvh] flex flex-col gap-6">
    <template v-if="isLoaded">
      <div class="flex items-center">
        <h2 class="text-3xl font-semibold min-w-70 text-(--text-color)">Detalles del local:</h2>
        <div class="w-full h-2 bg-(--secondary-color) rounded-md ml-4"></div>
      </div>

      <div class="w-full flex flex-col md:flex-row gap-6">
        <!-- Imagenes del local -->
        <div
            class="w-full md:w-2/3  flex flex-col shadow-lg bg-(--background-card-color) rounded-lg p-4 justify-center">
          <img :src="selectedPhoto" alt="Imagen del local"
               class="w-full h-full max-h-160 object-cover bg-(--button-color) rounded-lg"/>
          <div v-if="local.photoUrls.length > 1" class="flex gap-2 mt-4 overflow-x-auto">
            <img
                v-for="(photo, index) in local.photoUrls"
                :key="index"
                :src="photo"
                alt="Miniatura"
                class="w-32 h-24 object-cover rounded cursor-pointer border"
                :class="{ 'border-(--secondary-color)': selectedPhoto === photo }"
                @click="selectedPhoto = photo"
            />
          </div>
        </div>

        <!-- Panel lateral -->
        <div
            class="flex flex-col justify-center gap-6 shadow-lg bg-(--background-card-color) rounded-lg p-4 w-full md:w-1/3  overflow-y-auto">

          <!-- Información del local -->
          <h1 class="text-3xl font-semibold mt-4 text-(--text-color)">{{ local.localName }}</h1>
          <p class="text-2xl text-(--text-color)">{{ `📍 ${local.address}` }}</p>
          <p class="text-2xl text-(--text-color)"><span class="font-semibold">👨‍👨‍👧‍👧 Aforo: </span>{{ local.capacity }}
            personas</p>
          <p class="text-2xl text-(--text-color)"><span
              class="font-semibold">👑 Propietario: </span>{{ local.userUsername }}</p>
          <p class="text-2xl font-semibold text-(--text-color)">🔎 Descripción:</p>
          <p class="text-xl text-justify text-(--text-color)">{{ local.descriptionMessage }}</p>
          <div v-if="local.features[0] !== ''" class="flex flex-col gap-2">
            <p class="text-2xl font-bold text-(--text-color)">📕 Características</p>
            <ul class="list-disc list-inside text-justify">
              <li v-for="(feature, index) in local.features" :key="index" class="text-lg text-(--text-color)">{{
                  feature
                }}
              </li>
            </ul>
          </div>

          <!-- Opciones -->
          <h2 class="text-2xl font-semibold text-(--text-color)">Opciones:</h2>
          <div class="flex flex-col gap-5 text-xl">
            <RouterLink :to="`/comments/${local.id}`" class="text-(--primary-color) hover:underline">
              💬 Ver comentarios >
            </RouterLink>
            <RouterLink :to="`/report/${local.id}`" class="text-(--primary-color) hover:underline">
              ⚠ Reportar espacio >
            </RouterLink>
          </div>

          <!-- Fechas -->
          <div v-if="authenticationStore.userId !== local.userId" class="flex flex-col gap-5 text-(--text-color)">
            <div class="flex gap-4 justify-between items-center">
              <p class="text-xl">Fecha y hora de inicio:</p>
              <input
                  type="datetime-local"
                  class="w-1/2 p-2 border-2 caret-(--text-color) rounded-lg"
                  v-model="startDate"
              />
            </div>
            <p v-if="startDate && !isStartDateValid" class="text-red-500 text-sm">
              La fecha de inicio debe ser mayor o igual al momento actual.
            </p>

            <div class="flex gap-4 justify-between items-center">
              <p class="text-xl">Fecha y hora de fin:</p>
              <input
                  type="datetime-local"
                  class="w-1/2 p-2 border-2 caret-(--text-color) rounded-lg"
                  v-model="endDate"
              />
            </div>
            <p v-if="endDate && !isEndDateValid" class="text-red-500 text-sm">
              La fecha de fin debe ser posterior a la fecha de inicio.
            </p>
          </div>
          <div v-if="isFormValid"
               class="bg-(--background-card-color) text-(--text-color) p-4 rounded-lg mt-4 flex flex-col items-center">
            <h3 class="text-xl font-semibold mb-2">Cuenta del propietario:</h3>
            <ul class="flex flex-col gap-2">
              <p><span class="font-bold">Número de cuenta:</span> {{ bankAccounts.bankAccountNumber }}</p>
              <p><span class="font-bold">Número de cuenta interbancaria:</span> {{
                  bankAccounts.interbankAccountNumber
                }}</p>
              <p><span class="font-bold">Cantidad a depositar:</span> S/.{{ totalAmountToPay }}</p>
            </ul>
            <button @click="openUploadWidget" class="flex flex-col p-10 shadow-2xl hover:cursor-pointer">
              <img src="/svgs/camera.svg" alt="camera" class="w-1/2 max-w-30 mx-auto mt-4"/>
              <span class="text-center text-(--text-color) text-2xl">Adjuntar imagen del voucher</span>
            </button>
          </div>
          <!-- Botón -->
          <button v-if="authenticationStore.userId !== local.userId"
                  :disabled="!isFormValid || !voucherImageUrl"
                  class="bg-[var(--secondary-color)] rounded-md py-5 text-white text-xl hover:cursor-pointer hover:bg-[var(--secondary-color-hover)] transition duration-300 ease-in-out disabled:opacity-50 disabled:cursor-not-allowed"
                  @click="reserveLocal"
          >
            Reservar
          </button>
        </div>
      </div>
    </template>

    <!-- Skeleton mientras se carga -->
    <template v-else>
      <div class="flex items-center gap-4">
        <div class="h-8 w-1/4 skeleton"></div>
        <div class="h-2 w-full skeleton rounded-md"></div>
      </div>

      <div class="w-full flex flex-col md:flex-row gap-6">
        <!-- Skeleton de imagen principal -->
        <div class="w-full md:w-2/3 flex flex-col shadow-lg bg-(--background-card-color) rounded-lg p-4 gap-4">
          <!-- Imagen principal del local -->
          <div class="h-150 w-full skeleton"></div>
        </div>

        <!-- Skeleton panel lateral -->
        <div class="flex flex-col gap-4 shadow-lg bg-(--background-color) rounded-lg p-4 w-full md:w-1/3">
          <div class="h-8 w-2/3 skeleton"></div>
          <div class="h-6 w-1/2 skeleton"></div>
          <div class="h-6 w-3/4 skeleton"></div>
          <div class="h-6 w-1/2 skeleton"></div>
          <div class="h-24 w-full skeleton"></div>

          <div class="h-6 w-1/2 skeleton mt-4"></div>
          <div class="h-6 w-full skeleton"></div>
          <div class="h-6 w-full skeleton"></div>

          <div class="h-10 w-full skeleton mt-4"></div>
        </div>
      </div>
    </template>
  </main>
  <FooterComponent/>
</template>