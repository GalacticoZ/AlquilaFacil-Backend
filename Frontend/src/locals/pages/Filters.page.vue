<script setup>
  import { onMounted, ref } from 'vue';

  import NavbarComponent from '@/public/components/Navbar.component.vue'
  import { LocalCategoriesApiService } from '../services/local-categories-api.service';
  import LocalCategoryCardComponent from '../components/LocalCategoryCard.component.vue';
  import { useRouter } from 'vue-router';
import FooterComponent from '../../public/components/Footer.component.vue';
  const router = useRouter();
  const localCategories = ref([]);
  const localCategoriesApiService = new LocalCategoriesApiService();
  const localCategoryId = ref(1);
  const capacityOptionSelection = ref({ min: 1, max: 5 });
  const isLoaded = ref(false);
  const capacityOptions = [
    { min: 1, max: 5 },
    { min: 6, max: 10 },
    { min: 11, max: 20 },
    { min: 21, max: 30 },
    { min: 31, max: 50 },
    { min: 51, max: 100 },
  ]

  const selectCategory = (id) => {
    localCategoryId.value = id;
  };

  const goToFilterSearch = () => {
    router.push(`/filters-search/${localCategoryId.value}/${capacityOptionSelection.value.min}/${capacityOptionSelection.value.max}`);
  };

  onMounted(async () => {
    localCategories.value = await localCategoriesApiService.getAll();
    isLoaded.value = true;
  });
    
</script>

<template>
  <NavbarComponent />
  <main class="px-4 sm:px-8 md:px-10 lg:px-16 py-10 w-full min-h-[80dvh] flex flex-col items-center gap-10">
    <div class="w-full flex items-center justify-between">
      <h1 class="text-3xl text-center font-semibold text-(--text-color)">Aplicar filtros a tu búsqueda:</h1>
    </div>
    <div class="w-[80%] h-full flex flex-col gap-10 items-center text-(--text-color)">
      <div class="w-full grid grid-cols-1 md:grid-cols-2 gap-5">
        <div class="w-full flex flex-col items-center justify-between gap-2">
          <div class="w-full flex items-center justify-between">
            <h2 class="text-2xl">Categoría de local:</h2>
          </div>
          <template v-if="isLoaded">
            <div class="grid sm:grid-cols-2 gap-4 max-w-100">
              <LocalCategoryCardComponent
                v-for="localCategory in localCategories"
                :key="localCategory.id"
                :localCategory="localCategory"
                :isSelected="localCategory.id === localCategoryId"
                @click="selectCategory(localCategory.id)"
              />
            </div>
          </template>
          <template v-else>
            <div class="grid sm:grid-cols-2 gap-4 max-w-100">
              <LocalCategoryCardComponent v-for="n in 4" :key="n" :localCategory="{}" :isLoaded="false" />
            </div>
          </template>
        </div>
        <div class="w-full flex flex-col items-center gap-2">
          <div class="w-full flex items-center justify-between">
            <h2 class="text-2xl">Aforo:</h2>
          </div>
          <div class="flex flex-col justify-center items-center md:items-start gap-2 w-full h-full">
            <div v-for="option in capacityOptions" :key="option.min" class="flex items-center gap-5">
              <input
                type="radio"
                :id="`capacity-${option.min}`"
                :value="option"
                v-model="capacityOptionSelection"
                class="w-4 h-4 accent-(--primary-color)"
              />
              <label
                :for="`capacity-${option.min}`"
                class="ml-2 text-2xl text-(--text-color)"
              >
                {{ option.min }} - {{ option.max }} personas
              </label>
            </div>
          </div>
        </div>
      </div>

      <button @click="goToFilterSearch" type="button" class="py-6 bg-(--secondary-color) hover:bg-(--secondary-color-hover) transition text-white rounded-md hover:cursor-pointer w-2/3 text-xl">
        Aplicar filtros
      </button>
    </div>
  </main>
  <FooterComponent />
</template>