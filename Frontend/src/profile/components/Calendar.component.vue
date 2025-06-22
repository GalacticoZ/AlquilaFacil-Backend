<script setup>

import { ScheduleXCalendar } from '@schedule-x/vue';
import { createCalendar, createViewMonthGrid, createViewWeek, createViewDay } from '@schedule-x/calendar';
import '@schedule-x/theme-default/dist/calendar.css';
import { shallowRef, onMounted } from 'vue';
import { createEventsServicePlugin } from '@schedule-x/events-service';
import { useRouter } from 'vue-router';
import { ReservationsApiService } from '../../booking/services/reservations-api.service';
import { useAuthenticationStore } from '../../auth/services/authentication.store';

const router = useRouter();
const authenticationStore = useAuthenticationStore();
const reservationsApiService = new ReservationsApiService();

const eventsServicePlugin = createEventsServicePlugin();

const calendar = shallowRef(createCalendar({
  views: [
    createViewMonthGrid({
      name: 'month',
      displayName: 'Mes',
      default: true,
    }),
    createViewWeek({
      name: 'week',
      displayName: 'Semana',
    }),
    createViewDay({
      name: 'day',
      displayName: 'Día',
    }),
  ],
  selectedDate: new Date().toISOString().split('T')[0],
  selectedView: 'month',
  locale: 'es-ES',
  theme: {
    primaryColor: '#db9e49',
    textColor: '#000000',
    textColorHover: '#ffffff',
    backgroundColor: '#ffffff',
    backgroundColorHover: '#fb9e49',
  },
  calendars: {
    userReservation: {
      colorName: 'userReservation',
      lightColors: {
        main: '#d13333;',
        onContainer: '#ffffff',
        container: '#d13333',
      },
      darkColors: {
        main: '#d13333',
        onContainer: '#ffffff',
        container: '#d13333',
      },
    },
    localReservation: {
      colorName: 'localReservation',
      lightColors: {
        main: '#307fed',
        onContainer: '#ffffff',
        container: '#307fed',
      },
      darkColors: {
        main: '#307fed',
        onContainer: '#ffffff',
        container: '#307fed',
      },
    },
    premiumUserLocalReservation: {
      colorName: 'premiumUserLocalReservation',
      lightColors: {
        main: '#fb9e49',
        onContainer: '#ffffff',
        container: '#fb9e49',
      },
      darkColors: {
        main: '#fb9e49',
        onContainer: '#ffffff',
        container: '#fb9e49',
      },
      
    }
  },
  callbacks: {
    onEventClick: (event) => {
      localStorage.setItem('reservation', JSON.stringify(event));
      router.push(`/reservation-details`)
    },
  },
  plugins: [eventsServicePlugin],
}));

onMounted(async () => {
  const reservationsByUserId = await reservationsApiService.getByUserId(authenticationStore.userId);
  const reservationsByOwnerId = await reservationsApiService.getByOwnerId(authenticationStore.userId);
  const reservations = [...reservationsByUserId, ...reservationsByOwnerId];
  console.log('reservations', reservations);
  eventsServicePlugin.set(reservations);
  console.log(eventsServicePlugin.getAll());
});
</script>

<template>
  <ScheduleXCalendar :calendar-app="calendar" class="w-full xl:w-[80%] " />
</template>

<style>
  :root {
    --sx-color-background: var(--background-card-color) !important;
    --sx-color-primary: var(--secondary-color) !important;
    --sx-color-primary-container: var(--secondary-color) !important;
    --sx-color-surface-dim: var(--secondary-color) !important;
    --sx-internal-color-text: var(--text-color) !important;
    --sx-color-on-background: var(--text-color) !important;
    --sx-color-neutral: var(--text-color) !important;
    --sx-color-on-primary-container: var(--text-color) !important;
  }
  .sx-vue-calendar-wrapper {

    height: 80dvh;
  }
</style>