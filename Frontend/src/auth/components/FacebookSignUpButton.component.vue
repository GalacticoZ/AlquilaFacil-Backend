<script setup>
import { useAuthenticationStore } from '../services/authentication.store';

const signUpWithGoogle = () => {
  const authenticationStore = useAuthenticationStore();
  authenticationStore.signUpWithGoogle();
};
</script>

<template>
  <button type="button" class="max-w-12 max-h-12 flex items-center justify-center bg-white rounded-full hover:cursor-pointer object-cover" @click="signUpWithGoogle">
    <img src="/svgs/facebook-icon.svg" alt="Google Icon" class="size-15 max-w-[200%] select-none" />
  </button>
</template>