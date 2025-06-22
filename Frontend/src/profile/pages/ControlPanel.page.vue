<script setup>
import { useRouter } from 'vue-router';
import { useAuthenticationStore } from '../../auth/services/authentication.store.js';
import NavbarComponent from '@/public/components/Navbar.component.vue';

import { onMounted, ref } from 'vue';
import UpdateProfileComponent from '../components/UpdateProfile.component.vue';
import { ProfilesApiService } from '../services/profiles-api.service.js';
import ReportsMadeComponent from '../components/ReportsMade.component.vue';
import PublishedSpacesComponent from '../components/PublishedSpaces.component.vue';
import FooterComponent from '../../public/components/Footer.component.vue';
import ProfileSubscriptionComponent from '../components/ProfileSubscription.component.vue';
import FavoritesComponent from '../components/Favorites.component.vue';
import SubscriptionsManagementComponent from '../components/SubscriptionsManagement.component.vue';
import SupportComponent from '../components/Support.component.vue';

const router = useRouter();
const authenticationStore = useAuthenticationStore();

const userId = ref(null);
const profile = ref({});
const profilesApiService = new ProfilesApiService();

const options = {
  modificarPerfil: 'Modificar perfil',
  miSuscripcion: 'Mi suscripción',
  espaciosPublicados: 'Espacios publicados',
  ...(authenticationStore.userId === 1 && { gestionDeSuscripciones: 'Gestión de suscripciones' }),
  espaciosFavoritos: 'Espacios favoritos',
  soporte: 'Soporte',
  reportesRealizados: 'Reportes realizados',
};
const option = ref('Modificar perfil');

onMounted(async () => {
  userId.value = authenticationStore.userId;
  const profileResponse = await profilesApiService.getByUserId(userId.value);
  const bankAccountsResponse = await profilesApiService.getBankAccountsByUserId(userId.value);
  profile.value = { ...profileResponse, ...bankAccountsResponse };
});

const toggleTheme = () => {
  const current = document.documentElement.getAttribute("data-theme");
  const newTheme = current === "dark" ? "light" : "dark";
  document.documentElement.setAttribute("data-theme", newTheme);
  localStorage.setItem("theme", newTheme);
}

const handleOptionClick = (opt) => {
  option.value = opt;
};

const handleLogout = () => {
  authenticationStore.signOut(router);
};
</script>

<template>
  <NavbarComponent />
  <main class="w-full min-h-[80dvh] px-4 sm:px-8 md:px-10 lg:px-16 py-10 gap-6 flex flex-col">
    <h1 class="text-2xl text-(--text-color)">Panel de control</h1>
    <div class="w-full flex gap-4">
      <div class="flex items-center shadow-lg rounded-lg px-1 md:px-4 py-4 gap-2 w-1/4 max-h-180 bg-(--background-card-color)">
        <ul v-if="userId !== null" class="flex flex-col w-full gap-4 text-xs sm:text-sm md:text-lg">
          <li class="font-semibold flex p-2 w-full items-center justify-between text-(--text-color) rounded-lg transition">
            <span>Cambiar tema</span>
            <label class="relative inline-flex items-center cursor-pointer">
              <input type="checkbox" value="" class="sr-only peer" @change="toggleTheme()">
              <div class="w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-2 peer-focus:ring-red-500 rounded-full peer dark:bg-gray-600 peer-checked:bg-[var(--primary-color)] after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:after:translate-x-full peer-checked:after:border-white"></div>
            </label>
          </li>
          <li
            v-for="([key, label]) in Object.entries(options)"
            :key="key"
            class="font-semibold flex p-2 w-full items-center justify-between text-(--text-color) hover:bg-(--text-button-color) hover:cursor-pointer rounded-lg transition"
            @click="handleOptionClick(label)"
          >
            <span>{{ label }}</span>
            <svg class="hidden md:block w-5 h-5 md:w-10 md:h-10 fill-(--text-color)" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" id="forward">
              <g>
                <path d="M10 19a1 1 0 0 1-.64-.23 1 1 0 0 1-.13-1.41L13.71 12 9.39 6.63a1 1 0 0 1 .15-1.41 1 1 0 0 1 1.46.15l4.83 6a1 1 0 0 1 0 1.27l-5 6A1 1 0 0 1 10 19z"></path>
              </g>
            </svg>
          </li>
          <li class="font-semibold flex p-2 w-full items-center justify-between text-(--text-color) hover:bg-(--text-button-color) hover:cursor-pointer rounded-lg transition" @click="handleLogout">
            <span>Cerrar sesión</span>
          </li>
        </ul>
      </div>

      <div class="flex flex-col w-full shadow-lg rounded-lg p-4 bg-(--background-card-color)">
        <UpdateProfileComponent :profile="profile" v-if="option === options.modificarPerfil" />
        <ProfileSubscriptionComponent v-else-if="option === options.miSuscripcion" />
        <PublishedSpacesComponent v-else-if="option === options.espaciosPublicados" />
        <FavoritesComponent v-else-if="option === options.espaciosFavoritos" />
        <SupportComponent v-else-if="option === options.soporte" />
        <ReportsMadeComponent v-else-if="option === options.reportesRealizados" />
        <SubscriptionsManagementComponent
          v-else-if="option === options.gestionDeSuscripciones && userId === 1"
        />
      </div>
    </div>
  </main>
  <FooterComponent />
</template>
