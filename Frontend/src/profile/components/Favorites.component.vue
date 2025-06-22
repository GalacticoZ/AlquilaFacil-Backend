<script setup>
import { onMounted } from 'vue';
import LocalCardComponent from '../../locals/components/LocalCard.component.vue';
import { ref } from 'vue';

const favorites = ref([]);

onMounted(() => {
  favorites.value = JSON.parse(localStorage.getItem('favorites')) || [];
});

</script>

<template>
  <h2 class="text-xl md:text-2xl font-semibold text-center mb-6 text-(--text-color)">
      Encuentra con facilidad tu espacio ideal entre tus favoritos
  </h2>
  <div v-if="favorites.length > 0" class="grid grid-cols-1 md:grid-cols-2 gap-6">
    <LocalCardComponent
      v-for="local in favorites"
      :key="local.id"
      :local="local"
    />
  </div>
  <div v-else class="w-full flex items-center justify-center">
    <p class="text-lg text-(--text-color)">No tienes favoritos aún.</p>
  </div>
</template>