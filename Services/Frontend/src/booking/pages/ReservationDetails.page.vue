<script setup lang="ts">
import { onMounted, ref } from 'vue';
import NavbarComponent from '../../public/components/Navbar.component.vue';
import { LocalsApiService } from '../../locals/services/locals-api.service';
import { UsersApiService } from '../../shared/services/users-api.service';
import { LocalResponse } from '../../locals/model/local.response';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import { computed } from 'vue';
import { toLocalDateTimeString } from '../utils/toLocalDateTimeString';
import { ReservationsApiService } from '../services/reservations-api.service';
import { ReservationRequest } from '../models/reservation.request';
import FooterComponent from '../../public/components/Footer.component.vue';
import { useRouter } from 'vue-router';
import CreateCommentComponent from '../../locals/components/CreateComment.component.vue';

const localsApiService = new LocalsApiService();
const usersApiService = new UsersApiService();

const authenticationStore = useAuthenticationStore();
const router = useRouter();

const reservation = ref({});
const local = ref({});
const userUsername = ref('');
const userId = ref(authenticationStore.userId);
const postponeMinutes = ref(10);

const isFormValid = computed(() => postponeMinutes.value >= 10 && postponeMinutes.value <= 60);

onMounted(async() => {
  reservation.value = JSON.parse(localStorage.getItem('reservation'));
  const localResponse = await localsApiService.getById(reservation.value.localId);
  local.value = new LocalResponse(localResponse);
  userUsername.value = await usersApiService.getUsernameById(reservation.value.userId);
  localStorage.removeItem('reservation');
});

const postponeReservation = async () => {
  try {
    const currentStart = new Date(reservation.value.startDate);
    const currentEnd = new Date(reservation.value.endDate);

    const newStart = new Date(currentStart.getTime() + postponeMinutes.value * 60000);
    const newEnd = new Date(currentEnd.getTime() + postponeMinutes.value * 60000);

    reservation.value.startDate = toLocalDateTimeString(newStart);
    reservation.value.endDate = toLocalDateTimeString(newEnd);

    const reservationsApiService = new ReservationsApiService();
    const updateReservationRequest = new ReservationRequest(reservation.value);
    await reservationsApiService.update(reservation.value.id, updateReservationRequest);
    alert('Reserva actualizada correctamente');
    router.push('/calendar');
  } catch (error) {
    console.error('Error al posponer la reserva:', error);
    alert('Error al posponer la reserva. Por favor, inténtelo de nuevo más tarde.');
  }
};

const cancelReservation = async () => {
  try {
    const reservationsApiService = new ReservationsApiService();
    await reservationsApiService.delete(reservation.value.id);
    alert('Reserva cancelada correctamente');
    router.push('/calendar');
  } catch (error) {
    console.error('Error al cancelar la reserva:', error);
    alert('Error al cancelar la reserva. Por favor, inténtelo de nuevo más tarde.');
  }
};


</script>

<template>
  <NavbarComponent />
  <main class="px-4 sm:px-8 md:px-10 lg:px-16 py-10 w-full min-h-[90dvh] flex flex-col gap-6">
    <h1 class="text-2xl text-(--text-color)">Detalles de la reserva:</h1>

    <div class="w-full flex flex-col md:flex-row gap-6 text-(--text-color)">
      <div class="w-full md:w-2/3 flex flex-col shadow-lg bg-(--background-card-color) rounded-lg p-4">
        <img :src="local.photoUrl" alt="Imagen del local" class="w-full h-90 object-cover rounded-lg" />
        <h2 class="text-xl font-semibold mt-4">{{ local.localName }}</h2>
        <p class="text-lg mt-6">{{ `${local.streetAddress}, ${local.cityPlace}` }}</p>
        <p v-if="local.userId === userId" class="mt-3 text-xl"><span class="font-semibold">Arrendador de tu espacio: </span>{{ userUsername }}</p>
        <p v-else class="mt-3 text-xl"><span class="font-semibold">Propietario: </span>{{ local.userUsername }}</p>

        <div class="flex flex-col gap-5 mt-5">
          <div class="flex flex-col gap-2 justify-between items-start">
            <p class="text-xl font-semibold">Fecha y hora de inicio:</p>
            <p class="text-xl">{{`${new Date(reservation.startDate).toLocaleString('es-ES')}`}}</p>
          </div>
          <div class="flex flex-col gap-2 justify-between items-start">
            <p class="text-xl font-semibold">Fecha y hora de fin:</p>
            <p class="text-xl">{{`${new Date(reservation.endDate).toLocaleString('es-ES')}`}}</p>
          </div>
        </div>
      </div>

      <!-- Panel lateral -->
      <div class="flex flex-col justify-center gap-4 shadow-lg bg-white rounded-lg p-4 w-full md:w-1/3 max-h-180 overflow-y-auto">
        <h2 class="text-2xl font-semibold">Opciones:</h2>
        <div class="flex flex-col gap-5 text-xl">
          <RouterLink :to="`/comments/${local.id}`" class="text-[var(--primary-color)] hover:underline">
            Ver comentarios >
          </RouterLink>
          <RouterLink :to="`/report/${local.id}`" class="text-[var(--primary-color)] hover:underline">
            Reportar espacio >
          </RouterLink>
        </div>
        
        <div 
          v-if="new Date(reservation.startDate) >= new Date() && reservation.isSubscribe && local.userId === userId" 
          class="flex flex-col gap-5 text-xl"
        >
          <p class="text-(--primary-color)">Debido a que lo reservó un usuario premium, no se puede modificar el horario de reserva.</p>
        </div>
        <div v-else-if="new Date(reservation.startDate) >= new Date() && local.userId === userId" class="flex flex-col w-full gap-4">
          <h3 class="text-xl font-semibold">Modificar horario de reserva</h3>
          <p class="text-lg">Seleccione cuántos minutos desea posponer</p>
          <input
            type="number"
            v-model.number="postponeMinutes"
            :min="10"
            :max="60"
            step="5"
            class="w-full p-2 border border-gray-300 rounded-md"
          />

          <button
            @click="postponeReservation"
            :disabled="!isFormValid"
            class="bg-[var(--secondary-color)] rounded-md py-5 text-white text-xl hover:cursor-pointer hover:bg-[var(--secondary-color-hover)] transition duration-300 ease-in-out disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Posponer
          </button>
          
        </div>
        <div v-if="new Date(reservation.startDate) >= new Date() && local.userId === userId" class="flex flex-col w-full gap-4">
          <h3 class="text-xl font-semibold">Voucher de pago de reserva</h3>
          <a :href="reservation.voucherImageUrl" target="_blank">
            <img
              v-if="reservation.voucherImageUrl"
              :src="reservation.voucherImageUrl"
              alt="Voucher de pago"
              class="w-full h-40 object-cover rounded-lg cursor-zoom-in"
            />
          </a>
          <button
            @click="cancelReservation"
            class="bg-(--primary-color) rounded-md py-5 text-white text-xl hover:cursor-pointer hover:bg-(--primary-color-hover) transition duration-300 ease-in-out"
          >Cancelar reserva</button>
        </div>
        <CreateCommentComponent :localId="local.id" v-if="new Date(reservation.endDate) < new Date() && local.userId !== userId" />
      </div>
    </div>
  </main>
  <FooterComponent />
</template>