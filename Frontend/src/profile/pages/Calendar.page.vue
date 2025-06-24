<script setup>
import FooterComponent from '../../public/components/Footer.component.vue';
import NavbarComponent from '../../public/components/Navbar.component.vue';
import CalendarComponent from '../components/Calendar.component.vue';
</script>

<template>
  <NavbarComponent />
  <main class="w-full min-h-[80dvh] px-4 sm:px-8 md:px-10 lg:px-16 py-4 flex flex-col xl:flex-row gap-10">
    <CalendarComponent />
    <div class="flex flex-row xl:flex-col justify-center gap-4 w-full xl:max-w-80 min-h-60 px-4">
      <div class="flex flex-col">
        <div class="flex gap-2 items-center">
          <div class="min-w-10 h-10 bg-(--primary-color)"></div>
          <p class="text-xl text-(--text-color)">Tus reservas</p>
        </div>
      </div>
      <div class="flex flex-col">
        <div class="flex gap-2  items-center">
          <div class="min-w-10 h-10 bg-blue-500"></div>
          <p class="text-xl text-(--text-color)">Reserva de tu espacio</p>
        </div>
      </div>
      <div class="flex flex-col">
        <div class="flex gap-2 items-center">
          <div class="min-w-10 h-10 bg-(--secondary-color)"></div>
          <p class="text-xl text-(--text-color)">Reserva de un espacio por usuario premium</p>
        </div>
      </div>
    </div>
    
  </main>
  <FooterComponent />
</template>
