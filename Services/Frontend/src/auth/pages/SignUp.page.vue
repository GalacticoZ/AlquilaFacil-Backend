<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthenticationStore } from '../services/authentication.store';
import { SignUpRequest } from '../model/sign-up.request';
import GoogleSignUpButtonComponent from '../components/GoogleSignUpButton.component.vue';
import FacebookSignUpButtonComponent from '../components/FacebookSignUpButton.component.vue';
import InputFieldComponent from '../components/InputField.component.vue';

const router = useRouter();
const authenticationStore = useAuthenticationStore();

const checkTermsAndConditions = ref(false);

const formData = ref({
  username: '',
  name: '',
  fatherName: '',
  motherName: '',
  phoneNumber: '',
  documentNumber: '',
  dateOfBirth: '',
  email: '',
  password: '',
  passwordConfirmation: ''
});

const errors = ref({
  username: '',
  name: '',
  fatherName: '',
  motherName: '',
  phoneNumber: '',
  documentNumber: '',
  dateOfBirth: '',
  email: '',
  password: '',
  passwordConfirmation: '',
  general: ''
});

const requiredFields = [
  { key: 'username', message: 'El nombre de usuario es obligatorio.' },
  { key: 'name', message: 'El nombre es obligatorio.' },
  { key: 'fatherName', message: 'El apellido paterno es obligatorio.' },
  { key: 'motherName', message: 'El apellido materno es obligatorio.' },
  { key: 'phoneNumber', message: 'El teléfono es obligatorio.' },
  { key: 'documentNumber', message: 'El número de documento es obligatorio.' },
  { key: 'dateOfBirth', message: 'La fecha de nacimiento es obligatoria.' },
  { key: 'email', message: 'El correo electrónico es obligatorio.' },
  { key: 'password', message: 'La contraseña es obligatoria.' },
  { key: 'passwordConfirmation', message: 'La confirmación de contraseña es obligatoria.' }
];

// CAPTCHA
const captchaText = ref('');
const userCaptchaInput = ref('');
const isCaptchaValid = ref(false);

const generateCaptcha = () => {
  const chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
  captchaText.value = Array.from({ length: 5 }, () =>
    chars.charAt(Math.floor(Math.random() * chars.length))
  ).join('');
  userCaptchaInput.value = '';
  isCaptchaValid.value = false;
};

const validateCaptcha = () => {
  isCaptchaValid.value =
    userCaptchaInput.value.trim().toUpperCase() === captchaText.value;
};

generateCaptcha();

const validatePassword = (password) => {
  const symbols = "!@#$%^&*()_-+=[{]};:>|./?";
  return (
    password.length >= 8 &&
    /[A-Z]/.test(password) && // mayúscula
    /[a-z]/.test(password) && // minúscula
    /[0-9]/.test(password) && // número
    [...password].some(c => symbols.includes(c)) // símbolo
  );
};

const signUp = async () => {
  Object.keys(errors.value).forEach(k => errors.value[k] = '');

  for (const field of requiredFields) {
    if (!formData.value[field.key]) {
      errors.value[field.key] = field.message;
    }
  }

  if (formData.value.password && !validatePassword(formData.value.password)) {
    errors.value.password =
      'La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula, un número y un símbolo.';
  }

  if (
    formData.value.password &&
    formData.value.passwordConfirmation &&
    formData.value.password !== formData.value.passwordConfirmation
  ) {
    errors.value.passwordConfirmation = 'Las contraseñas no coinciden.';
  }

  if (formData.value.phoneNumber && formData.value.phoneNumber.length < 9) {
    errors.value.phoneNumber = 'El número de teléfono debe tener al menos 9 dígitos.';
  }

  if (!checkTermsAndConditions.value) {
    errors.value.general = 'Debes aceptar los términos y condiciones.';
  }

  if (!isCaptchaValid.value) {
    errors.value.general = 'El captcha es incorrecto.';
  }

  const hasErrors = Object.values(errors.value).some(msg => msg);
  if (hasErrors) return;

  try {
    const signUpRequest = new SignUpRequest(
      formData.value.username,
      formData.value.email,
      formData.value.password,
      formData.value.name,
      formData.value.fatherName,
      formData.value.motherName,
      formData.value.dateOfBirth,
      formData.value.documentNumber,
      formData.value.phoneNumber,
    );
    await authenticationStore.signUp(signUpRequest, router);
  } catch (error) {
    console.error('Error during sign-up:', error);
    errors.value.general = 'Error al registrarse. Por favor, inténtelo de nuevo más tarde.';
  }
};

const goToSignIn = () => {
  router.push(`/sign-in`);
};
</script>

<template>
  <section class="bg-(--primary-color) w-full min-h-[100dvh] px-4 sm:px-8 md:px-10 lg:px-16 py-20 flex flex-col justify-center items-center gap-2">
    <h1 class="text-white text-center text-4xl font-semibold">REGÍSTRATE</h1>

    <form @submit.prevent="signUp" class="grid grid-cols-1 md:grid-cols-2 gap-4 w-full max-w-180 mx-auto mt-10">

      <InputFieldComponent v-model="formData.username" :error="errors.username" placeholder="Nombre de usuario" />
      <InputFieldComponent v-model="formData.name" :error="errors.name" placeholder="Nombre"/>
      <InputFieldComponent v-model="formData.fatherName" :error="errors.fatherName" placeholder="Apellido paterno" />
      <InputFieldComponent v-model="formData.motherName" :error="errors.motherName" placeholder="Apellido materno" />
      <InputFieldComponent v-model="formData.phoneNumber" :error="errors.phoneNumber" placeholder="Número de teléfono" type="phone" />
      <InputFieldComponent v-model="formData.documentNumber" :error="errors.documentNumber" placeholder="Número de documento" type="phone"/>
      <InputFieldComponent v-model="formData.dateOfBirth"  :error="errors.dateOfBirth"  placeholder="Fecha de nacimiento" type="date" />
      <InputFieldComponent v-model="formData.email" :error="errors.email" placeholder="Correo electrónico" type="email" />
      <InputFieldComponent v-model="formData.password" :error="errors.password" placeholder="Contraseña" type="password" />
      <InputFieldComponent v-model="formData.passwordConfirmation" :error="errors.passwordConfirmation" placeholder="Confirmar contraseña" type="password" />

      <!-- CAPTCHA -->
      <div class="md:col-span-2 flex flex-col gap-2">
        <label class="text-white text-lg">Captcha</label>

        <div class="bg-gray-200 text-black font-mono text-xl text-center py-2 rounded tracking-widest select-none">
          {{ captchaText }}
        </div>

        <input
          v-model="userCaptchaInput"
          @input="validateCaptcha"
          placeholder="Ingresa el texto del captcha"
          class="p-3 rounded border border-white text-center text-white"
        />

        <p v-if="userCaptchaInput && !isCaptchaValid" class="text-white text-center text-sm">
          Captcha incorrecto. Intenta de nuevo.
        </p>

        <button type="button" @click="generateCaptcha" class="text-white text-sm text-center underline w-full">
          Generar nuevo captcha
        </button>
      </div>

      <!-- Checkbox -->
      <div class="flex items-center w-full md:col-span-2 gap-4">
        <input type="checkbox" v-model="checkTermsAndConditions" class="w-8 h-8" required />
        <label class="text-white">
          Al registrarse, acepta los <a href="https://alquiladorez.github.io/AlquilaFacil-LandingPage/service-terms.html"  target="_blank" class="font-bold">Términos y condiciones</a> y la <a href="https://alquiladorez.github.io/AlquilaFacil-LandingPage/privacy-policy.html"  target="_blank" class="font-bold">Política de privacidad</a>.
        </label>
      </div>

      <!-- Botón -->
      <button type="submit" class="bg-(--secondary-color) text-white p-4 rounded-md hover:cursor-pointer md:col-span-2">
        Regístrate ahora
      </button>

      <span v-if="errors.general" class="text-white text-sm text-center md:col-span-2">{{ errors.general }}</span>

      <p class="text-center text-base text-white md:col-span-2">
        ¿Ya tienes una cuenta?
      </p>

      <div class="w-full h-0.5 bg-white md:col-span-2"></div>

      <button type="button" class="bg-(--secondary-color) text-white p-4 rounded-md hover:cursor-pointer md:col-span-2" @click="goToSignIn">
        Iniciar sesión
      </button>
      <!--
      <p class="text-center text-base text-white md:col-span-2">
        o regístrate con:
      </p>

      <div class="flex justify-center items-center gap-2 md:col-span-2">
        <FacebookSignUpButtonComponent />
        <GoogleSignUpButtonComponent />
      </div>
      -->
    </form>
  </section>
</template>

<style scoped>
input[type="checkbox"] {
  appearance: none;
  -webkit-appearance: none;
  background-color: var(--primary-color);
  border: 2px solid var(--secondary-color);
  border-radius: 3px;
  cursor: pointer;
}

input[type="checkbox"]:checked {
  background-color: var(--secondary-color);
  border: 2px solid var(--primary-color);
  appearance: auto;
  accent-color: var(--secondary-color);
}
</style>
