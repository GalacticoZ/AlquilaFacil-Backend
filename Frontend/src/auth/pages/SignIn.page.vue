<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthenticationStore } from '../services/authentication.store';
import { SignInRequest } from '../model/sign-in.request';
import GoogleSignInButtonComponent from '../components/GoogleSignInButton.component.vue';
import FacebookSignInButtonComponent from '../components/FacebookSignInButton.component.vue';
import InputFieldComponent from '../components/InputField.component.vue';

const router = useRouter();
const authenticationStore = useAuthenticationStore();

const formData = ref({
  email: '',
  password: ''
});

const errors = ref({
  email: '',
  password: '',
  general: ''
});

const signIn = async () => {
  errors.value = {
    email: '',
    password: '',
    general: ''
  };

  if (!formData.value.email) {
    errors.value.email = 'El correo es obligatorio.';
  }
  if (!formData.value.password) {
    errors.value.password = 'La contraseña es obligatoria.';
  }

  if (errors.value.email || errors.value.password) return;

  try {
    const signInRequest = new SignInRequest(formData.value.email, formData.value.password);
    await authenticationStore.signIn(signInRequest, router);
  } catch (error) {
    console.error('Error during sign-in:', error);
    errors.value.general = 'Correo o contraseña incorrectos.';
  }
};

const goToSignUp = () => {
  router.push(`/sign-up`);
};
</script>


<template>
  <section class="bg-(--primary-color) w-full h-[100dvh] px-4 sm:px-8 md:px-10 lg:px-16 py-4 flex flex-col justify-center items-center gap-2">
    <h1 class="text-white text-center text-4xl font-semibold">INICIA SESIÓN</h1>

    <form @submit.prevent="signIn" class="flex flex-col gap-4 w-full max-w-160 mx-auto mt-10">
      <InputFieldComponent
        v-model="formData.email"
        :error="errors.email"
        placeholder="Correo electrónico"
        type="email"
        autocomplete="email"
      />
      <InputFieldComponent 
        v-model="formData.password"
        :error="errors.password"
        placeholder="Contraseña"
        type="password"
        autocomplete="current-password"
      />
      <button type="submit" class="bg-(--secondary-color) text-white p-4 rounded-md hover:cursor-pointer">
        Iniciar sesión
      </button>

      <span v-if="errors.general" class="text-white text-sm text-center">{{ errors.general }}</span>

      <p class="text-center text-base text-white">
        ¿Aún no tienes una cuenta?
      </p>
      <div class="w-full h-0.5 bg-white "></div>
      <button type="button" class="bg-(--secondary-color) text-white p-4 rounded-md hover:cursor-pointer" @click="goToSignUp">
        Regístrate
      </button>
      <!--
      <p class="text-center text-base text-white">
        o inicia sesión con:
      </p>
      <div class="flex justify-center items-center gap-2">
        <FacebookSignInButtonComponent />
        <GoogleSignInButtonComponent />
      </div>
      -->
    </form>
  </section>
</template>
