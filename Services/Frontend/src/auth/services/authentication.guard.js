import {useAuthenticationStore} from "./authentication.store.js";

/**
 * Authentication Guard
 * @summary
 * This guard is responsible for checking if the user is signed in or not.
 * If the user is not signed in and the route requires authentication, the user will be redirected to the sign-in page.
 * @param to - The route the user is trying to access
 * @param from - The route the user is coming from
 * @param next - The function to call to continue the navigation
 */
export const authenticationGuard = (to, from, next) => {
  const authenticationStore = useAuthenticationStore();
  const isAnonymous = !authenticationStore.isSignedIn;
  const publicRoutes = ['/sign-in', '/sign-up', '/about', '/page-not-found'];
  const routeRequiresToBeAuthenticated = !publicRoutes.includes(to.path);

  if (isAnonymous && routeRequiresToBeAuthenticated) next('/sign-in');
  else next();
}