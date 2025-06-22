<script setup>
import { onMounted, ref } from 'vue';
import { SubscriptionsApiService } from '../../subscriptions/services/subscriptions-api.service';

const subscriptionsApiService = new SubscriptionsApiService();
const subscriptions = ref([]);

const isLoading = ref(true);

onMounted(async () => {
  try {
    const subscriptionsResponse = await subscriptionsApiService.getAll();
    subscriptions.value = subscriptionsResponse
      .filter(subscription => subscription.subscriptionStatusId === 2)
      .map(subscription => ({
        ...subscription,
        activated: false,
      }));
  } catch (error) {
    console.error('Error al cargar las suscripciones:', error);
  } finally {
    isLoading.value = false;
  }
});

const activeSubscriptionStatus = async (subscriptionId) => {
  try {
    await subscriptionsApiService.activeSubscriptionStatus(subscriptionId);

    const index = subscriptions.value.findIndex(sub => sub.id === subscriptionId);
    if (index !== -1) {
      subscriptions.value[index].activated = true;
    }

  } catch (error) {
    console.error('Error al activar la suscripción:', error);
  }
};
</script>

<template>
  <div class="w-full p-4 flex flex-col gap-10">
    <h2 class="text-xl md:text-4xl font-bold text-center mb-6 text-(--text-color)">
      Control de suscripciones pendientes
    </h2>

    <div v-if="isLoading" class="text-center text-(--text-color)">
      Cargando suscripciones...
    </div>

    <div v-else>
      <div v-if="subscriptions.length > 0" class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div
          v-for="subscription in subscriptions"
          :key="subscription.id"
          class="w-full px-10 py-4 bg-(--background-color) shadow-md rounded-lg flex items-center justify-between transition duration-300 ease-in-out"
        >
          <div class="flex flex-col gap-4 w-full">
            <a :href="subscription.voucherImageUrl" target="_blank">
              <img
                v-if="subscription.voucherImageUrl"
                :src="subscription.voucherImageUrl"
                alt="Voucher de pago"
                class="w-full h-40 object-cover rounded-lg cursor-zoom-in"
              />
            </a>
            <button
              @click="activeSubscriptionStatus(subscription.id)"
              :disabled="subscription.activated"
              class="px-4 py-2 text-white font-medium bg-green-600 rounded-md hover:cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {{ subscription.activated ? 'Activada' : 'Activar suscripción' }}
            </button>
          </div>
        </div>
      </div>
      <div v-else class="text-center text-(--text-color)">
        No hay suscripciones pendientes.
      </div>
    </div>
  </div>
</template>
