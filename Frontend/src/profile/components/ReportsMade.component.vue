<script setup>
import { onMounted, ref } from 'vue';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import { ReportsApiService } from '../../locals/services/reports-api.service';
import { ReportResponse } from '../../locals/model/report.response';

const authenticationStore = useAuthenticationStore();
const reports = ref([]);
const reportsApiService = new ReportsApiService();

onMounted(async() => {
  const userId = authenticationStore.userId;
  reports.value = await reportsApiService.getByUserId(userId);
});

</script>

<template>
  <div class="w-full p-4 flex flex-col gap-10 text-(--text-color)">
    <h2 class="text-xl md:text-4xl font-bold text-center mb-6 ">
      Reportes realizados
    </h2>
    <div v-if="reports.length > 0" class="w-full grid grid-cols-1 md:grid-cols-2 gap-18 justify-center items-center">
      <div v-for="report in reports" :key="report.id" class="w-full p-10 bg-(--background-color) shadow-md rounded-lg flex items-center justify-between hover:cursor-pointer hover:shadow-2xl transition duration-300 ease-in-out">
        <div class="flex flex-col gap-4 w-full">
          <h3 class="text-xl font-semibold">Reporte ID: {{ report.id }}</h3>
          <h4 class="text-lg"><strong>Título:</strong> {{ report.title }}</h4>
          <p><strong>Descripción:</strong> {{ report.description }}</p>
        </div>
        <svg class="hidden md:block w-5 h-5 md:w-10 md:h-10" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" id="forward">
          <g>
            <path d="M10 19a1 1 0 0 1-.64-.23 1 1 0 0 1-.13-1.41L13.71 12 9.39 6.63a1 1 0 0 1 .15-1.41 1 1 0 0 1 1.46.15l4.83 6a1 1 0 0 1 0 1.27l-5 6A1 1 0 0 1 10 19z" fill="white"></path>
          </g>
        </svg>
      </div>
    </div>
    <div v-else class="w-full flex flex-col items-center justify-center gap-4">
      <p class="text-lg text-center">No tienes reportes realizados.</p>
    </div>
  </div>
</template>