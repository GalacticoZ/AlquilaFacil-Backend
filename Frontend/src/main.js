import { createApp } from 'vue'
import './style.css'
import router from "@/public/router/router.js";
import {createPinia} from "pinia";
import App from './App.vue'
import { firebaseApp } from './auth/services/authentication.firebase';


const pinia = createPinia();

firebaseApp;

const app = createApp(App);
app.use(pinia);
app.use(router);
app.mount('#app')
