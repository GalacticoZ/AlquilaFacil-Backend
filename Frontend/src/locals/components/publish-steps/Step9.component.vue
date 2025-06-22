<script setup>
const props = defineProps({
  price: String,
});

const emit = defineEmits([
  'update:price',
]);
</script>

<template>
  <h1 class="text-3xl text-center font-semibold text-(--text-color)">Ponle un precio a tu espacio</h1>
  <p class="text-lg text-center text-(--text-color)">Podrás cambiarlo en cualquier momento.</p>
  <div class="flex justify-center items-center gap-4 w-full max-w-120 text-(--text-color)">
    <label for="price">S/.</label>
    <input
      type="number"
      id="price"
      :value="props.price"
      @input="emit('update:price', $event.target.value)"
      class="border border-gray-300 rounded px-4 py-4 text-right text-2xl max-w-50"
      placeholder="Ingresa tu país"
      min="1"
    />
  </div>
  <p class="text-lg text-center text-(--text-color)">Por hora</p>
  
</template>