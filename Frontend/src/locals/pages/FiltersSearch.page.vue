<script setup>
  import { onMounted, ref } from 'vue';

  import NavbarComponent from '@/public/components/Navbar.component.vue'
  import LocalCardComponent from '../components/LocalCard.component.vue';
  import { LocalsApiService } from '../services/locals-api.service';
import { useRoute } from 'vue-router';
import FooterComponent from '../../public/components/Footer.component.vue';

  const locals = ref([]);
  const localsApiService = new LocalsApiService();
  const route = useRoute();
  const isLoaded = ref(false);

  onMounted(async () => {
    const localCategoryId = parseInt(route.params.localCategoryId);
    const minCapacity = parseInt(route.params.minCapacity);
    const maxCapacity = parseInt(route.params.maxCapacity);
    locals.value = await localsApiService.getByCategoryAndCapacityRange(localCategoryId, minCapacity, maxCapacity);
    isLoaded.value = true;
  });

</script>

<template>
  <NavbarComponent />
  <main class="px-4 sm:px-8 md:px-10 lg:px-16 py-10 w-full min-h-[80dvh] flex flex-col gap-6">
    <div class="w-full flex items-center justify-between">
      <h1 class="text-3xl text-center font-semibold text-(--text-color)">Resultados de búsqueda:</h1>
    </div>
    
    <template v-if="isLoaded">
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        <LocalCardComponent v-for="local in locals" :key="local.id" :local="local" />      
      </div>
    </template>
    <template v-else>
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        <LocalCardComponent v-for="n in 6" :key="n" :local="{}" :isLoaded="false" />
      </div>
    </template>

  </main>
  <FooterComponent />
</template>