<script setup>
import { useAuthenticationStore } from '../services/authentication.store';

const signInWithGoogle = () => {
  const authenticationStore = useAuthenticationStore();
  authenticationStore.signInWithGoogle();
};
</script>

<template>
  <button type="button" class="max-w-12 max-h-12 flex items-center justify-center bg-white rounded-full hover:cursor-pointer object-cover" @click="signInWithGoogle">
    <img src="/svgs/facebook-icon.svg" alt="Google Icon" class="size-15 max-w-[200%] select-none" />
  </button>
</template>