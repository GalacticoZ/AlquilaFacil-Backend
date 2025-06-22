<script setup>
import { ref } from 'vue';
import NavbarComponent from '@/public/components/Navbar.component.vue'
import Step1Component from '../components/publish-steps/Step1.component.vue';
import Step2Component from '../components/publish-steps/Step2.component.vue';
import Step3Component from '../components/publish-steps/Step3.component.vue';
import Step4Component from '../components/publish-steps/Step4.component.vue';
import Step5Component from '../components/publish-steps/Step5.component.vue';
import Step6Component from '../components/publish-steps/Step6.component.vue';
import Step7Component from '../components/publish-steps/Step7.component.vue';
import Step8Component from '../components/publish-steps/Step8.component.vue';
import Step9Component from '../components/publish-steps/Step9.component.vue';
import Step10Component from '../components/publish-steps/Step10.component.vue';
import { LocalsApiService } from '../services/locals-api.service';
import { useRouter } from 'vue-router';
import { LocalRequest } from '../model/local.request';
import { LocalResponse } from '../model/local.response';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import FooterComponent from '../../public/components/Footer.component.vue';

const router = useRouter();
const authenticationStore = useAuthenticationStore();

const localData = ref({
  localName: '',
  descriptionMessage: '',
  country: '',
  city: '',
  district: '',
  street: '',
  price: 0,
  capacity: '',
  photoUrls: [],
  features: [],
  localCategoryId: 1,
  userId: '',
})

const pageNumber = ref(1);

const changePage = (number) => {
  const nextPage = pageNumber.value + number;
  if (nextPage > 0 && nextPage <= 10) {
    pageNumber.value = nextPage;
    console.log(localData.value)
  } else {
    console.warn(`Página fuera de rango: ${nextPage}`);
  }
};

const publishLocal = async () => {
  localData.value.userId = authenticationStore.userId;
  console.log(localData.value)
  const allFieldsFilled = Object.entries(localData).every(([key, value]) => {
    return value !== null && value !== undefined && value !== '' && value !== 0;
  });

  if (!allFieldsFilled) {
    console.log('No se han completado todos los campos requeridos.');
    router.push('/error');
    return;
  }
  const localsApiService = new LocalsApiService();
  
  const localRequest = new LocalRequest(localData.value);
  try {
    const response = await localsApiService.create(localRequest);
    const localResponse = new LocalResponse(response);
    router.push(`/local/${localResponse.id}`);
  }
  catch (error) {
    console.error('Error al publicar el local:', error);
    router.push('/error');
  }
}
</script>

<template>
  <NavbarComponent />
  <main class="w-full min-h-[80dvh] px-4 sm:px-8 md:px-10 lg:px-16 py-20 flex flex-col justify-center items-center gap-4">
    <Step1Component v-if="pageNumber === 1" />
    <button @click="changePage(1)" v-if="pageNumber === 1" class="bg-(--secondary-color) text-white rounded-md px-36 py-4 mt-10 text-xl hover:bg-(--secondary-color-hover) hover:cursor-pointer">Comencemos</button>
    <Step2Component v-if="pageNumber === 2" />
    <Step3Component v-if="pageNumber === 3" v-model:localCategoryId="localData.localCategoryId" />
    <Step4Component v-if="pageNumber === 4" v-model:country="localData.country" v-model:city="localData.city" v-model:district="localData.district" v-model:street="localData.street"/>
    <Step5Component v-if="pageNumber === 5" />
    <Step6Component v-if="pageNumber === 6" v-model:photoUrls="localData.photoUrls" />
    <Step7Component v-if="pageNumber === 7" v-model:localName="localData.localName" v-model:descriptionMessage="localData.descriptionMessage" v-model:capacity="localData.capacity" v-model:features="localData.features" />
    <Step8Component v-if="pageNumber === 8" />
    <Step9Component v-if="pageNumber === 9" v-model:price="localData.price" />
    <div v-if="pageNumber > 1 && pageNumber<10" class="flex gap-10 mt-10">
      <button @click="changePage(-1)" class="bg-(--background-color) text-(--text-color) rounded-md px-5 md:px-36 py-4 text-xl border-2 border-(--secondary-color)  hover:border-(--secondary-color-hover) hover:cursor-pointer">Atrás</button>
      <button @click="changePage(1)" class="bg-(--secondary-color) text-white rounded-md px-5 md:px-36 py-4 text-xl hover:bg-(--secondary-color-hover) hover:cursor-pointer">Siguiente</button>
    </div>
    <Step10Component v-if="pageNumber === 10" v-model:localData="localData" @update:localData="localData = $event"/>
    <div v-if="pageNumber === 10" class="flex gap-10 mt-10">
      <button @click="changePage(-1)" class="bg-(--background-color) text-(--text-color) rounded-md px-5 md:px-36 py-4 mt-10 text-xl border-2 border-(--secondary-color) hover:border-(--secondary-color-hover) hover:cursor-pointer">Atrás</button>
      <template v-if="localData.localName && localData.descriptionMessage && localData.country && localData.city && localData.district && localData.street && localData.price > 0 && localData.capacity > 0 && localData.photoUrls.length > 0 && localData.features.length > 0">
        <button @click="publishLocal" class="bg-(--secondary-color) text-white rounded-md px-5 md:px-36 py-4 mt-10 text-xl hover:bg-(--secondary-color-hover) hover:cursor-pointer">Publicar local</button>
      </template>
    </div>
  </main>
  <FooterComponent />
</template>