<script setup>
import { useAuthenticationStore } from '../services/authentication.store';

const signInWithGoogle = () => {
  const authenticationStore = useAuthenticationStore();
  authenticationStore.signInWithGoogle();
};
</script>

<template>
  <button type="button" class="max-w-12 max-h-12 flex items-center justify-center gap-2 p-2 bg-white rounded-full hover:cursor-pointer" @click="signInWithGoogle">
    <img src="/svgs/google-icon.svg" alt="Google Icon" class="size-12 select-none" />
  </button>
</template>