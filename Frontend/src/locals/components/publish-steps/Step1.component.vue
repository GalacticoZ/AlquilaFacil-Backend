<script setup>
import { useRouter } from 'vue-router';
import { ProfilesApiService } from '../../../profile/services/profiles-api.service';
import { useAuthenticationStore } from '../../../auth/services/authentication.store';
import { onMounted } from 'vue';

const profilesApiService = new ProfilesApiService();
const authenticationStore = useAuthenticationStore();
const router = useRouter();

onMounted(async () => {
  const userId = authenticationStore.userId;
  const bankAccounts = await profilesApiService.getBankAccountsByUserId(userId);
  console.log(bankAccounts);
  if (bankAccounts.bankAccountNumber.length == 0 || bankAccounts.interbankAccountNumber.length == 0) {
    alert("No tienes cuentas bancarias registradas. Por favor, registra una cuenta bancaria para poder recibir pagos por la reserva de tu local.");
    router.push("/control-panel");
  }
});
</script>


<template>
  <h1 class="text-3xl text-center font-semibold text-(--text-color)">Empezar a usar AlquilaFácil es muy sencillo</h1>
  <p class="text-lg text-center text-(--text-color)">Completa los siguientes pasos para registrar tu espacio en la aplicación.</p>
  <div class="flex flex-col gap-6 max-w-90 text-(--text-color)">
    <div class="flex gap-4 items-center justify-start">
      <img
        src="/images/step-1-1.png"
        alt="Paso 1"
        class="size-24"
      />
      <div class="w-full flex flex-col justify-center">
        <h2 class="text-2xl text-center">1. Describe tu espacio</h2>
        <p class="text-center">Comparte algunos datos básicos</p>
      </div>
    </div>
    <div class="flex gap-4 items-center justify-start">
      <img
        src="/images/step-1-2.png"
        alt="Paso 2"
        class="size-24"
      />
      <div class="w-full flex flex-col justify-center">
        <h2 class="text-2xl text-center">2. Haz que destaque</h2>
        <p class="text-center">Agrega una foto y un título a tu espacio, nosotros nos encargamos del resto</p>
      </div>
    </div>
    <div class="flex gap-4 items-center justify-start">
      <img
        src="/images/step-1-3.png"
        alt="Paso 3"
        class="size-24"
      />
      <div class="w-full flex flex-col justify-center">
        <h2 class="text-2xl text-center">3. Terminar y publicar</h2>
        <p class="text-center">Agrega las últimas configuraciones y publica tu espacio</p>
      </div>
    </div>
  </div>
</template>