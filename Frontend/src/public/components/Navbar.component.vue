<script setup>
import { RouterLink } from 'vue-router';
import DistrictSearchBarComponent from '@/locals/components/DistrictSearchBar.component.vue';
</script>

<template>
  <nav class="text-(--navbar-text-color) bg-(--primary-color) w-full h-[10dvh] sticky flex justify-between px-1 sm:px-8 md:px-10 lg:px-16 gap-4 md:gap-6">
    <RouterLink to="/" class="flex items-center gap-2 transition font-semibold">
      <img src="/logo.png" alt="logo" class="min-w-14 h-14" />
      <p class="hidden sm:flex text-2xl">AlquilaFácil</p>
    </RouterLink>

    <DistrictSearchBarComponent />

    <div class="flex items-center justify-end">

      <ul class="flex xl:hidden gap-2 text-sm sm:text-base lg:text-xl items-center">
        <li class="transition hover:text-(--navbar-text-color-hover)">
          <RouterLink to="/publish" class="cursor-pointer">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0,0,256,256"
              class="size-7 md:size-10 fill-white hover:fill-(--navbar-text-color-hover) transition"
            >
              <g transform="scale(5.12,5.12)">
                <path d="M25,2c-12.6907,0 -23,10.3093 -23,23c0,12.69071 10.3093,23 23,23c12.69071,0 23,-10.30929 23,-23c0,-12.6907 -10.30929,-23 -23,-23zM25,4c11.60982,0 21,9.39018 21,21c0,11.60982 -9.39018,21 -21,21c-11.60982,0 -21,-9.39018 -21,-21c0,-11.60982 9.39018,-21 21,-21zM24,13v11h-11v2h11v11h2v-11h11v-2h-11v-11z"/>
              </g>
            </svg>
          </RouterLink>
        </li>
        <li class="transition hover:text-(--navbar-text-color-hover)">
          <RouterLink to="/calendar" class="cursor-pointer">
            <svg
            class="size-7 md:size-10 fill-white hover:fill-(--navbar-text-color-hover) transition"
            version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" xmlns:xlink="http://www.w3.org/1999/xlink" enable-background="new 0 0 512 512" stroke="#ffffff"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <g> <g> <path d="M490.6,43H381.1V11h-20.9v32H151.7V11h-20.9v32H21.4C15.2,43,11,47.2,11,53.6v436.7c0,6.4,4.2,10.7,10.4,10.7h469.1 c6.3,0,10.4-4.3,10.4-10.7V53.6C501,47.2,496.8,43,490.6,43z M480.1,479.7H31.9V64.3h99v32h20.9v-32h208.5v32h20.9v-32h99V479.7z"></path> <path d="m84,170.8c-6.3,0-10.4,4.3-10.4,10.7v245c0,6.4 4.2,10.7 10.4,10.7h344c6.3,0 10.4-4.3 10.4-10.7v-245c0-6.4-4.2-10.7-10.4-10.7h-344zm333.6,245h-323.2v-223.7h323.2v223.7z"></path> <rect width="20.9" x="214.3" y="232.6" height="21.3"></rect> <rect width="20.9" x="276.9" y="232.6" height="21.3"></rect> <rect width="20.9" x="339.4" y="232.6" height="21.3"></rect> <rect width="20.9" x="151.7" y="293.3" height="21.3"></rect> <rect width="20.9" x="214.3" y="293.3" height="21.3"></rect> <rect width="20.9" x="276.9" y="293.3" height="21.3"></rect> <rect width="20.9" x="339.4" y="293.3" height="21.3"></rect> <rect width="20.9" x="151.7" y="355.1" height="21.3"></rect> <rect width="20.9" x="214.3" y="355.1" height="21.3"></rect> <rect width="20.9" x="276.9" y="355.1" height="21.3"></rect> <rect width="20.9" x="339.4" y="355.1" height="21.3"></rect> </g> </g> </g></svg>
          </RouterLink>
        </li>
        <li class="transition hover:text-(--navbar-text-color-hover)">
          <RouterLink to="/notifications" class="cursor-pointer">
            <svg viewBox="0 0 24 24"
            class="size-7 md:size-10 stroke-white hover:stroke-(--navbar-text-color-hover) transition"
            fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M12.02 2.90991C8.70997 2.90991 6.01997 5.59991 6.01997 8.90991V11.7999C6.01997 12.4099 5.75997 13.3399 5.44997 13.8599L4.29997 15.7699C3.58997 16.9499 4.07997 18.2599 5.37997 18.6999C9.68997 20.1399 14.34 20.1399 18.65 18.6999C19.86 18.2999 20.39 16.8699 19.73 15.7699L18.58 13.8599C18.28 13.3399 18.02 12.4099 18.02 11.7999V8.90991C18.02 5.60991 15.32 2.90991 12.02 2.90991Z"  stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round"></path> <path d="M13.87 3.19994C13.56 3.10994 13.24 3.03994 12.91 2.99994C11.95 2.87994 11.03 2.94994 10.17 3.19994C10.46 2.45994 11.18 1.93994 12.02 1.93994C12.86 1.93994 13.58 2.45994 13.87 3.19994Z" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M15.02 19.0601C15.02 20.7101 13.67 22.0601 12.02 22.0601C11.2 22.0601 10.44 21.7201 9.90002 21.1801C9.36002 20.6401 9.02002 19.8801 9.02002 19.0601"  stroke-width="1.5" stroke-miterlimit="10"></path> </g></svg>
          </RouterLink>
        </li>
        <li class="transition hover:text-(--navbar-text-color-hover)">
          <RouterLink to="/control-panel" class="cursor-pointer">
            <svg viewBox="0 0 24 24"
             class="size-7 md:size-10 stroke-white hover:stroke-(--navbar-text-color-hover) transition"
             fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M12.12 12.78C12.05 12.77 11.96 12.77 11.88 12.78C10.12 12.72 8.71997 11.28 8.71997 9.50998C8.71997 7.69998 10.18 6.22998 12 6.22998C13.81 6.22998 15.28 7.69998 15.28 9.50998C15.27 11.28 13.88 12.72 12.12 12.78Z" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M18.74 19.3801C16.96 21.0101 14.6 22.0001 12 22.0001C9.40001 22.0001 7.04001 21.0101 5.26001 19.3801C5.36001 18.4401 5.96001 17.5201 7.03001 16.8001C9.77001 14.9801 14.25 14.9801 16.97 16.8001C18.04 17.5201 18.64 18.4401 18.74 19.3801Z" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M12 22C17.5228 22 22 17.5228 22 12C22 6.47715 17.5228 2 12 2C6.47715 2 2 6.47715 2 12C2 17.5228 6.47715 22 12 22Z" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>
          </RouterLink>
        </li>
      </ul>

      <ul class="hidden xl:flex gap-1 lg:gap-2 text-sm sm:text-base lg:text-xl items-center ">
        <li class="transition px-1 lg:px-3 hover:text-(--navbar-text-color-hover)">
          <RouterLink to="/publish" class="cursor-pointer">Publicar</RouterLink>
        </li>
        <li class="transition px-1 lg:px-3 hover:text-(--navbar-text-color-hover)">
          <RouterLink to="/calendar" class="cursor-pointer">Calendario</RouterLink>
        </li>
        <li class="transition px-1 lg:px-3 hover:text-(--navbar-text-color-hover)">
          <RouterLink to="/notifications" class="cursor-pointer">Notificaciones</RouterLink>
        </li>
        <li class="transition px-1 lg:px-3 hover:text-(--navbar-text-color-hover)">
          <RouterLink to="/control-panel" class="cursor-pointer">Perfil</RouterLink>
        </li>
      </ul>
    </div>
  </nav>
</template>

