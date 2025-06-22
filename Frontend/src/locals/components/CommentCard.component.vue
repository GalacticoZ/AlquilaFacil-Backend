<script setup>
import { defineProps } from 'vue';
import StarRating from 'vue3-star-ratings';
import { CommentResponse } from '../model/comment.response';

const props = defineProps({
  comment: Object
});

const commentResponse = new CommentResponse(props.comment);
</script>

<template>
  <div class="flex items-center shadow-lg rounded-lg p-4 gap-2 min-h-40 text-(--text-color) bg-(--background-card-color)">
    <div class="bg-(--primary-color) min-w-18 h-18 flex justify-center items-center rounded-full p-2">
      <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M12.12 12.78C12.05 12.77 11.96 12.77 11.88 12.78C10.12 12.72 8.71997 11.28 8.71997 9.50998C8.71997 7.69998 10.18 6.22998 12 6.22998C13.81 6.22998 15.28 7.69998 15.28 9.50998C15.27 11.28 13.88 12.72 12.12 12.78Z" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M18.74 19.3801C16.96 21.0101 14.6 22.0001 12 22.0001C9.40001 22.0001 7.04001 21.0101 5.26001 19.3801C5.36001 18.4401 5.96001 17.5201 7.03001 16.8001C9.77001 14.9801 14.25 14.9801 16.97 16.8001C18.04 17.5201 18.64 18.4401 18.74 19.3801Z" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M12 22C17.5228 22 22 17.5228 22 12C22 6.47715 17.5228 2 12 2C6.47715 2 2 6.47715 2 12C2 17.5228 6.47715 22 12 22Z" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>
    </div>
    <div class="flex flex-col w-full p-2 gap-2 text-center md:text-start">
      <div class="flex flex-col md:flex-row justify-between items-center">
        <span class="text-sm md:text-lg font-semibold text-(--text-color)">{{commentResponse.userUsername}}</span>
        <StarRating :model-value="commentResponse.rating" :star-color="'#fb9e49'" :inactive-color="'white'":number-of-stars="5" :star-size="20" :disable-click="true" />
      </div>
      <p class="text-sm md:text-lg text-(--text-color)">{{commentResponse.text}}</p>
    </div>
  </div>
</template>