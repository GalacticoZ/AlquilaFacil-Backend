<script setup>
import { ref, watch } from 'vue';

const props = defineProps({
  label: String,
  modelValue: String,
});
const emit = defineEmits(['update:modelValue']);

const editing = ref(false);
const localValue = ref(props.modelValue);

watch(() => props.modelValue, (newVal) => {
  localValue.value = newVal;
});

const toggleEdit = () => {
  if (editing.value) {
    emit('update:modelValue', localValue.value);
  }
  editing.value = !editing.value;
};
</script>

<template>
  <div class="flex justify-between items-center gap-4 border-b border-(--secondary-color) pb-2 col-span-1 md:col-span-2 lg:col-span-1">
    <div class="flex flex-col justify-end-safe gap-3 w-full">
      <label class="text-base font-medium text-(--text-color)">{{ label }}:</label>

      <div v-if="!editing">
        <p class="text-(--text-color) text-sm md:text-xl">{{ modelValue || '—' }}</p>
      </div>

      <div v-else>
        <input
          type="text"
          v-model="localValue"
          class="border rounded p-2 w-full text-(--text-color)"
          :placeholder="'Editar ' + label"
        />
      </div>
    </div>

    <button
      type="button"
      :class="editing ? 'bg-(--secondary-color) hover:bg-(--secondary-color-hover) text-white' : 'bg-(--button-color) hover:bg-(--button-color-hover) transition text-black'"
      class="text-base px-3 py-2 rounded hover:cursor-pointer"
      @click="toggleEdit"
    >
      {{ editing ? 'Actualizar' : 'Editar' }}
    </button>
  </div>
</template>
