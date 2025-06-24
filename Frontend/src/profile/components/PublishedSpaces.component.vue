<script setup>
import { onMounted, ref } from 'vue';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import { LocalsApiService } from '../../locals/services/locals-api.service';

const authenticationStore = useAuthenticationStore();
const localsApiService = new LocalsApiService();

const locals = ref([]);

onMounted(async () => {
  try {
    const userId = authenticationStore.userId;
    const response = await localsApiService.getUserLocals(userId);
    locals.value = response;
  } catch (error) {
    console.error('Error fetching published spaces:', error);
  }
});

</script>

<template>
  <div v-if="locals" class="w-full p-4 flex flex-col gap-10">
    <h2 class="text-xl md:text-4xl font-bold text-center mb-6 text-(--text-color)">
      Espacios publicados
    </h2>
    <div class="w-full flex justify-center items-center">
      <div v-if="locals.length === 0" class="w-full flex items-center justify-center">
        <p class="text-lg text-center text-(--text-color)">No tienes espacios publicados aún.</p>
      </div>
    </div>  
    <div v-for="local in locals" :key="local.id" class="w-full px-10 py-4 text-(--text-color) bg-(--background-color) shadow-md rounded-lg flex items-center justify-between hover:cursor-pointer hover:shadow-2xl transition duration-300 ease-in-out" @click="$router.push(`/local/${local.id}`)">
      <div class="flex gap-4 w-full items-center">
        <img :src="local.photoUrls[0]" alt="Local Image" class="w-32 h-32 object-cover rounded-lg" />
        <div class="flex flex-col justify-center gap-4 w-full">
          <h3 class="text-2xl font-semibold">{{ local.localName }}</h3>
          <p class="text-xl font-semibold">📍 {{ local.address }}</p>
          <p class="text-lg">{{ `💵 Precio: S/.${local.price} por hora` }}</p>
        </div>
        <p class="w-60 text-right text-(--secondary-color-dark)">
          <span class="text-lg font-semibold">Ver espacio ></span>
        </p>
      </div>
    </div>
  </div>
</template>