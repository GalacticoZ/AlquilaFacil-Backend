<script setup >
import { onMounted } from 'vue';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import NavbarComponent from '../../public/components/Navbar.component.vue';
import { NotificationsApiService } from '../services/notifications-api.service';
import { ref } from 'vue';
import FooterComponent from '../../public/components/Footer.component.vue';

const authenticationStore = useAuthenticationStore();
const notificationsApiService = new NotificationsApiService();

const notifications = ref([]);
const isLoaded = ref(false);

onMounted (async () => {
  const userId = authenticationStore.userId;
  notifications.value = await notificationsApiService.getByUserId(userId);
  isLoaded.value = true;
});

</script>

<template>
  <NavbarComponent />
  <main class="px-4 sm:px-8 md:px-10 lg:px-16 py-10 w-full min-h-[80dvh] flex flex-col gap-6">
    <h1 class="text-2xl text-(--text-color)">Notificaciones</h1>
    <div v-if="isLoaded && notifications.length === 0" class="text-center text-(--text-color)">
      No tienes notificaciones.
    </div>
    <div v-else class="flex flex-col gap-4">
      <div v-for="notification in notifications" :key="notification.id" class="bg-(--background-color) shadow-md rounded-lg p-4">
        <h2 class="text-lg font-semibold">{{ notification.title }}</h2>
        <p class="text-(--text-color)">{{ notification.content }}</p>
      </div>
    </div>
  </main>
  <FooterComponent />
</template>