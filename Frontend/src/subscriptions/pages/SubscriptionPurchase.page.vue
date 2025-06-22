<script setup lang="ts">

import NavbarComponent from '../../public/components/Navbar.component.vue';
import FooterComponent from '../../public/components/Footer.component.vue';
import { onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { SubscriptionPlansApiService } from '../services/subscription-plans-api.service';
import { ProfilesApiService } from '../../profile/services/profiles-api.service';
import { SubscriptionsApiService } from '../services/subscriptions-api.service';
import { SubscriptionRequest } from '../model/subscription.request';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import { cloudinaryWidget } from '../../shared/components/cloudinary-widget';

const route = useRoute();
const router = useRouter();
const authenticationStore = useAuthenticationStore();
const subscriptionPlansApiService = new SubscriptionPlansApiService();
const profilesApiService = new ProfilesApiService();
const subscriptionsApiService = new SubscriptionsApiService();
const plan = ref({});
const bankAccounts = ref({});
const voucherImageUrl = ref('');

onMounted(async () => {
  const planId = parseInt(route.params.planId);
  const subscriptionPlans = await subscriptionPlansApiService.getAll();
  bankAccounts.value = await profilesApiService.getBankAccountsByUserId(1);
  plan.value = subscriptionPlans.find(plan => plan.id === planId);
});

const openUploadWidget = async () => {
  try {
    const secureUrls = await cloudinaryWidget();
    console.log("URL segura:", secureUrls);
    voucherImageUrl.value = secureUrls[0];
  } catch (error) {
    console.error("Error al subir imagen:", error);
  }
};

const purchaseSubscription = async () => {
  try {
    const subscriptionRequest = new SubscriptionRequest({
      planId: plan.value.id,
      userId: authenticationStore.userId,
      voucherImageUrl: voucherImageUrl.value,
    });
    await subscriptionsApiService.create(subscriptionRequest);
    alert('Tu voucher será validada para culminar la compra de tu suscripción');
    router.push('/');
  } catch (error) {
    alert('Error al realizar la compra de la suscripción:', error);
  }
};

</script>

<template>
  <NavbarComponent />
  <main class="flex flex-col justify-center items-center gap-4 w-full h-full p-4 sm:p-8 md:p-10 lg:p-16 text-(--text-color)">
    <h1 class="text-3xl text-center font-semibold">Compra de suscripción</h1>
    <p class="text-lg text-center">Para poder realizar la compra del plan, adjunta la foto del voucher de pago.</p>
    <p class="text-lg text-center">El plan seleccionado es: {{ plan.name }}</p>
    <p class="text-xl text-center font-semibold">El precio a pagar por este plan es de S/.{{ plan.price }}</p>
    <p class="text-lg text-center">Puedes realizar el pago a las siguientes cuentas bancarias:</p>
    <ul>
      <li class="text-xl font-semibold">Cuenta bancaria: {{ bankAccounts.bankAccountNumber }}</li>
      <li class="text-xl font-semibold">Cuenta interbancaria: {{ bankAccounts.interbankAccountNumber }}</li>
    </ul>
    <div class="flex flex-col gap-8 justify-center mt-6">
      <button @click="openUploadWidget" class="flex flex-col p-10 shadow-xl hover:cursor-pointer">
        <img src="/svgs/camera.svg" alt="camera" class="w-1/2 max-w-30 mx-auto mt-4" />
        <span class="text-center text-(--text-color) text-2xl">Adjuntar imagen del voucher</span>
      </button>
      <button
        :disabled="!voucherImageUrl"
        class="bg-(--secondary-color) rounded-md py-5 text-white text-xl hover:cursor-pointer hover:bg-(--secondary-color-hover) transition duration-300 ease-in-out disabled:opacity-50 disabled:cursor-not-allowed"
        @click="purchaseSubscription"
      >
      Comprar suscripción
      </button>
    </div>
  </main>
  <FooterComponent />
</template>