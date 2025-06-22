<script setup>
import { onMounted } from 'vue';
import { ProfilesApiService } from '../services/profiles-api.service';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import { ref } from 'vue';
import { SubscriptionPlansApiService } from '../../subscriptions/services/subscription-plans-api.service';
import { useRouter } from 'vue-router';

const profilesApiService = new ProfilesApiService();
const authenticationStore = useAuthenticationStore();
const subscriptionPlansApiService = new SubscriptionPlansApiService();
const subscriptionStatus = ref('');
const subscriptionPlans = ref([]);
const router = useRouter();
const isLoaded = ref(false);


onMounted(async () => {
  try {
    subscriptionPlans.value = await subscriptionPlansApiService.getAll();
    subscriptionStatus.value = await profilesApiService.getSubscriptionStatusByUserId(authenticationStore.userId);
    console.log(subscriptionStatus.value);
  } catch (error) {
    console.error('Error al cargar datos de suscripción:', error);
  } finally {
    isLoaded.value = true;
  }
});

const goToPurchasePage = (planId) => {
  router.push(`/purchase-subscription/${planId}`);
};

</script>


<template>
  <div class="w-full p-4 flex flex-col gap-5 justify-center items-center">
    <h2 class="text-xl md:text-4xl font-bold text-center mb-6 text-(--text-color)">
      Estado de suscripción
    </h2>

    <div v-if="!isLoaded" class="text-center text-(--text-color) py-10">
      Cargando datos de suscripción...
    </div>

    <div v-else-if="subscriptionStatus === 'No subscription found'" class="flex flex-col gap-4 justify-center items-center text-(--text-color)">
      <p class="text-center text-2xl">Obtén una suscripción para disfrutar de los beneficios.</p>
      <button
        v-for="plan in subscriptionPlans"
        :key="plan.id"
        class="flex flex-col justify-center items-center px-4 py-20 gap-2 rounded-md shadow-lg hover:shadow-2xl hover:cursor-pointer transition duration-300"
        @click="goToPurchasePage(plan.id)"
      >
        <img src="https://www.supercoloring.com/sites/default/files/fif/2017/05/gold-star-paper-craft.png" alt="Plan Image" class="w-32 h-32 object-cover rounded-lg" />
        <p class="text-2xl font-semibold">{{ plan.name }}</p>
        <p>{{ plan.service }}</p>
        <p>Tendrás preferencias al momento de reservar. Por ejemplo, no podrán posponer tu horario de reserva.</p>
        <p class="text-lg font-semibold">{{ `Precio: S/.${plan.price}` }}</p>
      </button>
    </div>

    <div v-else-if="subscriptionStatus === 'Pending'" class="flex flex-col gap-4 justify-center items-center">
      <p class="text-center text-2xl">Tu suscripción está pendiente de validación.</p>
      <p class="text-lg font-semibold text-center">Tu plan es: {{ subscriptionPlans[0].name }}</p>
      <img src="https://www.supercoloring.com/sites/default/files/fif/2017/05/gold-star-paper-craft.png" alt="Plan Image" class="w-32 h-32 object-cover rounded-lg" />
      <p class="text-lg font-semibold text-center">{{ subscriptionPlans[0].service }}</p>
      <p class="text-lg font-semibold text-center">Tu voucher será validado para culminar la compra de tu suscripción.</p>
    </div>

    <div v-else-if="subscriptionStatus === 'Active'" class="flex flex-col gap-4 justify-center items-center">
      <p class="text-center text-2xl">Tu suscripción está activa.</p>
      <p class="text-lg font-semibold text-center">Tu plan es: {{ subscriptionPlans[0].name }}</p>
      <img src="https://www.supercoloring.com/sites/default/files/fif/2017/05/gold-star-paper-craft.png" alt="Plan Image" class="w-32 h-32 object-cover rounded-lg" />
      <p class="text-lg font-semibold text-center">{{ subscriptionPlans[0].service }}</p>
    </div>
  </div>
</template>
