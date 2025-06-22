<script setup>
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import NavbarComponent from '../../public/components/Navbar.component.vue';
import CommentCardComponent from '../components/CommentCard.component.vue';

import { CommentsApiService } from '../services/comments-api.service';
import FooterComponent from '../../public/components/Footer.component.vue';


const route = useRoute();
const comments = ref([]);
const commentsApiService = new CommentsApiService();

onMounted(async() => {
  const localId = parseInt(route.params.localId);
  comments.value = await commentsApiService.getAllByLocalId(localId);
})
</script>

<template>
  <NavbarComponent />
  <main class="px-4 sm:px-8 md:px-10 lg:px-16 py-10 w-full min-h-[80dvh] flex flex-col gap-6">
    <div class="w-full flex items-center justify-between">
      <h1 class="text-2xl text-(--text-color)">Comentarios del espacio:</h1>
    </div>
    
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <CommentCardComponent v-for="comment in comments" :key="comment.id" :comment="comment" />      
    </div>

  </main>
  <FooterComponent />
</template>