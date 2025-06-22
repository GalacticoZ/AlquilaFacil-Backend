import { createRouter, createWebHistory } from 'vue-router'
import { titleBrandingGuard } from '@/shared/services/title-branding.guard';
import { authenticationGuard } from '../../auth/services/authentication.guard';

const routes = [
  { path: '/', redirect: '/search' },
  { path: '/sign-in', meta: { title: "Iniciar sesión"}, component: () => import('@/auth/pages/SignIn.page.vue') },
  { path: '/sign-up', meta: { title: "Registrarse"}, component: () => import('@/auth/pages/SignUp.page.vue') },
  { path: '/search', meta: { title: "Búsqueda"}, component: () => import('@/locals/pages/Search.page.vue') },
  { path: '/filters', meta: { title: "Filtros"}, component: () => import('@/locals/pages/Filters.page.vue') },
  { path: '/filters-search/:localCategoryId/:minCapacity/:maxCapacity', meta: { title: "Búsqueda por filtros"}, component: () => import('@/locals/pages/FiltersSearch.page.vue') },
  { path: '/district/:districtId', meta: { title: "Búsqueda por distrito"}, component: () => import('@/locals/pages/DistrictSearch.page.vue') },
  { path: '/publish', meta: { title: "Publicar"}, component: () => import('@/locals/pages/Publish.page.vue') },
  { path: '/local/:id', meta: { title: "Información de espacio" }, component: () => import('@/locals/pages/LocalDetail.page.vue') },
  { path: '/comments/:localId', meta: { title: "Comentarios"}, component: () => import('@/locals/pages/Comments.page.vue') },
  { path: '/calendar' , meta: { title: "Calendario"}, component: () => import('@/profile/pages/Calendar.page.vue') },
  { path: '/notifications', meta: { title: "Notificaciones"}, component: () => import('@/notification/pages/Notifications.page.vue') },
  { path: '/control-panel', meta: { title: "Panel de control"}, component: () => import('@/profile/pages/ControlPanel.page.vue') },
  { path: '/reservation-details', meta: { title: "Detalles de reserva"}, component: () => import('@/booking/pages/ReservationDetails.page.vue') },
  { path: '/purchase-subscription/:planId', meta: { title: "Comprar suscripción"}, component: () => import('@/subscriptions/pages/SubscriptionPurchase.page.vue') },
  { path: '/report/:localId', meta: { title: "Reportar"}, component: () => import('@/locals/pages/Report.page.vue') },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.beforeEach((to, from, next) => {
  titleBrandingGuard(to, from, (titleNext) => {
    // Si titleBranding guard redirecciona o cancela
    if (typeof titleNext === 'string' || titleNext === false) {
      return next(titleNext);
    }
    authenticationGuard(to, from, next);
  });
});
export default router;