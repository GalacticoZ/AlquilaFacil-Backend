<script setup>
import StarRating from 'vue3-star-ratings';
import { CommentsApiService } from '../services/comments-api.service';
import { useAuthenticationStore } from '../../auth/services/authentication.store';
import { useRouter } from 'vue-router';
import { CommentRequest } from '../model/comment.request';
import { ref } from 'vue';

const props = defineProps({
  localId: Number
});

const commentsApiService = new CommentsApiService();
const authenticationStore = useAuthenticationStore();
const router = useRouter();
const text = ref('');
const rating = ref(0);

const publishComment = async () => {
  try {
    const commentRequest = new CommentRequest({
    userId: authenticationStore.userId,
    localId: props.localId,
    text: text.value,
    rating: Math.floor(rating.value)
  });
    await commentsApiService.create(commentRequest);
    alert('Comentario publicado correctamente');
    router.push(`/comments/${props.localId}`);
  } catch (error) {
    alert('Error al publicar el comentario. Por favor, inténtalo de nuevo más tarde.');
  }
};

</script>
<template>
  <div class="flex flex-col w-full gap-4 mt-5 text-(--text-color)">
    <h3 class="text-2xl font-semibold">Publicar comentario</h3>

    <label for="commentText" class="text-lg font-medium">Escribe tu comentario:</label>
    <textarea
      id="commentText"
      v-model="text"
      placeholder="Comparte tu experiencia..."
      class="w-full p-3 border border-gray-300 rounded-md resize-y min-h-[100px] text-(--text-color)"
    ></textarea>

    <label class="text-lg font-medium">Calificación:</label>
    <StarRating
      v-model="rating"
      :max-rating="5"
      :increment="1"
      :star-size="30"
      inactive-color="#d1d5db"
      active-color="#fbbf24"
    />

    <button
      @click="publishComment"
      class="bg-(--secondary-color) rounded-md py-5 text-white text-xl hover:cursor-pointer transition duration-300 ease-in-out"
    >
      Publicar
    </button>
  </div>
</template>
