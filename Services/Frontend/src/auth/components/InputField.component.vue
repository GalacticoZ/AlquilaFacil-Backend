<script setup>
defineProps({
  modelValue: String,
  placeholder: String,
  error: String,
  type: {
    type: String,
    default: 'text'
  },
  autocomplete: String
});

defineEmits(['update:modelValue']);
</script>

<template>
  <div class="flex flex-col gap-1">
    <input
      :type="type"
      :placeholder="placeholder"
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
      class="p-4 rounded-md bg-white"
      required
      :autocomplete="autocomplete"
    />
    <span v-if="error" class="text-white text-sm">{{ error }}</span>
  </div>
</template>