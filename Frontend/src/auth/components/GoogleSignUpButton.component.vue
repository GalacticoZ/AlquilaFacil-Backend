<script setup>
import { useAuthenticationStore } from '../services/authentication.store';


const signUpWithGoogle = () => {
  const authenticationStore = useAuthenticationStore();
  authenticationStore.signUpWithGoogle();
};
</script>

<template>
  <button type="button" class="max-w-12 max-h-12 flex items-center justify-center gap-2 p-2 bg-white rounded-full hover:cursor-pointer"  @click="signUpWithGoogle">
    <img src="/svgs/google-icon.svg" alt="Google Icon" class="size-12 select-none" />
  </button>
</template>