<script setup>
import { ref, watchEffect } from 'vue';
import { useRouter } from 'vue-router';
import { LocalResponse } from '../model/local.response';

const props = defineProps({
  local: Object,
  isLoaded: {
    type: Boolean,
    default: true
  }
});

const router = useRouter();
const localResponse = new LocalResponse(props.local);

const isFavorite = ref(false);

const checkIfFavorite = () => {
  const favorites = JSON.parse(localStorage.getItem('favorites')) || [];
  isFavorite.value = favorites.some(fav => fav.id === localResponse.id);
};

watchEffect(() => {
  checkIfFavorite();
});

const goToLocal = () => {
  router.push(`/local/${props.local.id}`);
};

const addToFavorites = async () => {
  const favorites = JSON.parse(localStorage.getItem('favorites')) || [];
  const existingIndex = favorites.findIndex(fav => fav.id === localResponse.id);

  if (existingIndex !== -1) {
    favorites.splice(existingIndex, 1);
    isFavorite.value = false;
  } else {
    favorites.push(localResponse);
    isFavorite.value = true;
  }

  localStorage.setItem('favorites', JSON.stringify(favorites));
};
</script>

<template>
  <div v-if="isLoaded"
    class="flex flex-col shadow-lg rounded-lg hover:shadow-xl transition duration-300 ease-in-out hover:cursor-pointer bg-(--background-card-color)"
    @click="goToLocal"
  >
    <img :src="localResponse.photoUrls[0]" alt="Local Image" class="w-full h-60 object-cover rounded-lg" />
    <div class="p-4 rounded-lg flex justify-between items-center">
      <div class="flex flex-col gap-1">
        <h2 class="text-xl font-semibold text-(--text-color)">{{ localResponse.localName }}</h2>
        <p class="text-(--text-color)">{{ localResponse.address }}</p>
      </div>
      <button
        @click.stop="addToFavorites"
        class="rounded-full size-14 flex justify-center items-center hover:cursor-pointer"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="30"
          height="30"
          viewBox="0 0 256 256"
          :fill="isFavorite ? '#FFD700' : 'white'"
          :stroke="isFavorite ? '#FFD700' : 'black'"
        >
          <g transform="translate(1.4066 1.4066) scale(2.81 2.91)">
            <path
              d="M 69.671 88.046 c -0.808 0 -1.62 -0.195 -2.37 -0.59 L 45 75.732 L 22.7 87.456 c -1.727 0.907 -3.779 0.757 -5.356 -0.388 c -1.577 -1.146 -2.352 -3.052 -2.023 -4.972 l 4.259 -24.832 L 1.539 39.678 c -1.396 -1.361 -1.889 -3.358 -1.287 -5.213 c 0.603 -1.854 2.176 -3.181 4.105 -3.461 l 24.932 -3.622 L 40.44 4.79 C 41.303 3.041 43.05 1.955 45 1.955 c 0 0 0 0 0.001 0 c 1.949 0 3.696 1.086 4.559 2.834 l 11.15 22.592 l 24.932 3.623 c 1.93 0.28 3.503 1.606 4.105 3.461 c 0.603 1.854 0.109 3.851 -1.287 5.213 L 70.419 57.264 l 4.26 24.832 c 0.329 1.921 -0.446 3.827 -2.023 4.972 C 71.764 87.717 70.721 88.046 69.671 88.046 z"
            />
          </g>
        </svg>
      </button>
    </div>
  </div>
  <div v-else class="flex flex-col gap-4 shadow-lg rounded-lg p-4 animate-pulse bg-(--background-card-color)">
    <div class="w-full h-48 skeleton rounded-lg"></div>
    <div class="h-6 skeleton rounded w-3/4"></div>
    <div class="h-4 skeleton rounded w-full"></div>
    <div class="h-4 bg-skeleton rounded w-5/6"></div>
  </div>
</template>
