<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import { LocalsApiService } from '../services/locals-api.service';

const query = ref('');
const districts = ref([]);
const filteredDistricts = ref([]);

const localsApiService = new LocalsApiService();

const router = useRouter();

onMounted(async () => {
  try {
    districts.value = await localsApiService.getAllDistricts();
    filteredDistricts.value = districts.value;
  } catch (error) {
    console.error('Error al obtener los distritos:', error);
  }
});

watch(query, () => {
  filteredDistricts.value = districts.value.filter(district =>
    district.toLowerCase().includes(query.value.toLowerCase())
  );
  console.log(filteredDistricts.value);
});


const goToDistrict = (district) => {
  localStorage.setItem('district', district);
  router.push(`/district/${district}`);
};
</script>

<template>
  <div class="relative flex items-center justify-center w-full h-full">
    <div class="px-2 py-2 flex items-center justify-between  
    w-full max-w-180
    h-12 sm:h-14 rounded-lg border-2 border-white gap-1">
      <input
        v-model="query"
        type="text"
        placeholder="Buscar distrito..."
        class="w-full md:p-2 outline-0"
      />
      <svg xmlns="http://www.w3.org/2000/svg" class="size-8" viewBox="0,0,256,256">
      <g fill="#ffffff" fill-rule="nonzero" stroke="none" stroke-width="1" stroke-linecap="butt" stroke-linejoin="miter" stroke-miterlimit="10" stroke-dasharray="" stroke-dashoffset="0" font-family="none" font-weight="none" font-size="none" text-anchor="none" style="mix-blend-mode: normal"><g transform="scale(5.12,5.12)"><path d="M21,3c-9.37891,0 -17,7.62109 -17,17c0,9.37891 7.62109,17 17,17c3.71094,0 7.14063,-1.19531 9.9375,-3.21875l13.15625,13.125l2.8125,-2.8125l-13,-13.03125c2.55469,-2.97656 4.09375,-6.83984 4.09375,-11.0625c0,-9.37891 -7.62109,-17 -17,-17zM21,5c8.29688,0 15,6.70313 15,15c0,8.29688 -6.70312,15 -15,15c-8.29687,0 -15,-6.70312 -15,-15c0,-8.29687 6.70313,-15 15,-15z"></path></g></g>
      </svg>
    </div>
    <ul v-if="query && filteredDistricts.length > 0" 
      class="text-[var(--text-color)] absolute top-20 bg-[var(--background-color)] 
      w-full max-w-180 rounded-md shadow-lg max-h-60 overflow-auto z-10"
    >
      <li
        v-for="(district, index) in filteredDistricts"
        :key="index"
        @click="goToDistrict(district)"
        class="cursor-pointer px-4 py-2 hover:bg-gray-200"
      >
        {{ district }}
      </li>
    </ul>
  </div>
</template>