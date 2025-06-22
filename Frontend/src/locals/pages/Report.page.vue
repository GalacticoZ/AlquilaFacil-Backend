<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import FooterComponent from '../../public/components/Footer.component.vue';
import NavbarComponent from '../../public/components/Navbar.component.vue';
import { LocalsApiService } from '../services/locals-api.service';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import { ReportRequest } from '../model/report.request';
import { ReportsApiService } from '../services/reports-api.service';

const route = useRoute();
const router = useRouter();
const authenticationStore = useAuthenticationStore();
const localsApiService = new LocalsApiService();
const reportsApiService = new ReportsApiService();
const local = ref({});

const title = ref('');
const description = ref('');
const errors = ref({
  title: '',
  description: '',
  general: ''
});

onMounted(async () => {
  try {
    const id = parseInt(route.params.localId);
    local.value = await localsApiService.getById(id);
  } catch (error) {
    console.error('Error al cargar los datos del local:', error);
  }
});

const publishReport = async () => {
  errors.value.title = '';
  errors.value.description = '';
  errors.value.general = '';

  if (!title.value.trim()) {
    errors.value.title = 'El título es obligatorio.';
  }
  if (!description.value.trim()) {
    errors.value.description = 'La descripción es obligatoria.';
  }

  if (errors.value.title || errors.value.description) return;

  try {
    const reportRequest = new ReportRequest({
      userId: authenticationStore.userId,
      localId: local.value.id,
      title: title.value,
      description: description.value,
    });
    await reportsApiService.create(reportRequest);
    alert('Reporte enviado correctamente');
    router.push(`/local/${local.value.id}`);
  } catch (error) {
    console.error('Error al enviar el reporte:', error);
    errors.value.general = 'Error al enviar el reporte. Por favor, inténtelo de nuevo más tarde.';
  }
};
</script>

<template>
  <NavbarComponent />
  <main class="px-4 sm:px-8 md:px-10 lg:px-16 py-10 w-full min-h-[80dvh] flex flex-col gap-6">
    <h1 class="text-3xl text-center font-semibold">Reportar {{ local.localName }}</h1>

    <div class="max-w-xl mx-auto w-full flex flex-col gap-4">
      <label class="flex flex-col gap-2">
        <span class="font-medium text-2xl">Título del reporte</span>
        <input
          v-model="title"
          type="text"
          placeholder="Escribe el título..."
          class="border border-gray-300 p-4 rounded text-xl"
        />
        <span class="text-red-500 text-sm" v-if="errors.title">{{ errors.title }}</span>
      </label>

      <label class="flex flex-col gap-2">
        <span class="font-medium text-2xl">Descripción del reporte</span>
        <textarea
          v-model="description"
          placeholder="Describe el problema o situación..."
          class="border border-gray-300 p-4 rounded text-xl min-h-60"
        ></textarea>
        <span class="text-red-500 text-sm" v-if="errors.description">{{ errors.description }}</span>
      </label>

      <span class="text-red-600" v-if="errors.general">{{ errors.general }}</span>

      <button
        @click="publishReport"
        class="bg-(--primary-color) text-white py-4 px-4 rounded hover:bg-(--primary-color-hover) hover:cursor-pointer transition-colors w-full text-xl"
      >
        Enviar reporte
      </button>
    </div>
  </main>
  <FooterComponent />
</template>