<script setup>
const props = defineProps({
  localCategory: Object,
  isSelected: Boolean,
  isLoaded: {
    type: Boolean,
    default: true
  }
});
</script>

<template>
  <div v-if="isLoaded"
    class="flex flex-col p-4 gap-6 rounded-md shadow-2xl border-4 hover:cursor-pointer transition-all bg-white"
    :class="isSelected ? 'border-(--primary-color)' : 'border-transparent'"
  >
    <img
      :src="localCategory.photoUrl"
      alt="Local Category Image"
      class="w-80 h-40 object-contain rounded-lg"
    />
    <p class="text-center text-xl text-black">
      {{ localCategory.name }}
    </p>
  </div>
  <div v-else class="flex flex-col p-4 gap-6 rounded-md shadow-2xl border-4 border-transparent">
    <div class="skeleton w-80 max-w-full h-40"></div>
    <p class="skeleton text-center text-xl w-3/4 mx-auto h-6"></p>
  </div>
</template>
