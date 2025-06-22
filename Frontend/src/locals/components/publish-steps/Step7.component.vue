<script setup>
import { ref } from 'vue';

const props = defineProps({
  localName: String,
  descriptionMessage: String,
  capacity: String,
  features: Array,
});

const emit = defineEmits([
  'update:localName',
  'update:descriptionMessage',
  'update:capacity',
  'update:features'
]);

const newFeature = ref('');

const addFeature = () => {
  const trimmed = newFeature.value.trim();
  if (trimmed && !props.features.includes(trimmed)) {
    emit('update:features', [...props.features, trimmed]);
    newFeature.value = '';
  }
};

const removeFeature = (index) => {
  const updated = [...props.features];
  updated.splice(index, 1);
  emit('update:features', updated);
};

</script>

<template>
  <h1 class="text-3xl text-center font-semibold text-(--text-color)">Ahora, añade los detalles de tu espacio</h1>
  <p class="text-lg text-center text-(--text-color)">Los títulos cortos funcionan mejor. No te preocupes, puedes modificarlo más adelante.</p>
  <div class="flex flex-col w-full max-w-140 text-(--text-color)">
    <label for="localName" class="bg-(--background-color) w-22 text-lg font-semibold relative top-4 left-2 px-2">Nombre</label>
    <input
      type="text"
      id="localName"
      :value="props.localName"
      @input="emit('update:localName', $event.target.value)"
      class="border border-gray-300 rounded px-4 py-4 text-(--text-color)"
      placeholder="Ingresa el nombre de tu local"
    />
  </div>

  <div class="flex flex-col w-full max-w-140 text-(--text-color)">
    <label for="descriptionMessage" class="bg-(--background-color) w-30 text-lg font-semibold relative top-4 left-2 px-2">Descripción</label>
    <input
      type="text"
      id="descriptionMessage"
      :value="props.descriptionMessage"
      @input="emit('update:descriptionMessage', $event.target.value)"
      class="border border-gray-300 rounded px-4 py-4 text-(--text-color)"
      placeholder="Ingresa la descripción de tu local"
    />
  </div>

  <div class="flex flex-col w-full max-w-140 text-(--text-color)">
    <label for="capacity" class="bg-(--background-color) w-16 text-lg font-semibold relative top-4 left-2 px-2">Aforo</label>
    <input
      type="number"
      id="capacity"
      :value="props.capacity"
      @input="emit('update:capacity', $event.target.value)"
      class="border border-gray-300 rounded px-4 py-4 text-(--text-color)"
      placeholder="Ingresa el aforo"
      min="1"
    />
  </div>
  <div class="flex flex-col w-full max-w-140 mt-6 text-(--text-color)">
    <label class="text-lg font-semibold">Características</label>
    <div class="flex items-center gap-2 mt-2">
      <input
        v-model="newFeature"
        @keyup.enter="addFeature"
        type="text"
        placeholder="Agregar característica (Dimensiones del local, servicios, etc.)"
        class="flex-1 border border-gray-300 rounded px-3 py-2 text-(--text-color)"
      />
      <button
        @click="addFeature"
        class="text-xl font-bold px-3 py-2 rounded bg-(--button-color-hover) hover:bg-(--text-button-color) border transition"
      >
        +
      </button>
    </div>

    <ul class="mt-4 space-y-2">
      <li
        v-for="(feature, index) in props.features"
        :key="index"
        class="flex items-center justify-between border-b pb-1"
      >
        <span>• {{ feature }}</span>
        <button
          @click="removeFeature(index)"
          class="text-red-500 hover:text-red-700 text-sm"
          title="Eliminar"
        >
          🗑️
        </button>
      </li>
    </ul>
  </div>
</template>