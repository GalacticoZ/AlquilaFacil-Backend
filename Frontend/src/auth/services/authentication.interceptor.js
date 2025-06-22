import {useAuthenticationStore} from "./authentication.store.js";

/**
 * Authentication Interceptor
 *
 * @summary
 * This function is an interceptor that adds the Authorization header to the request if the user is signed in.
 * @param config - Axios request configuration
 * @returns {*} - Modified axios request configuration
 */
export const authenticationInterceptor = (config) => {
  const authenticationStore = useAuthenticationStore();
  const isSignedIn = authenticationStore.isSignedIn;
  if (isSignedIn) {
    config.headers.Authorization = `Bearer ${authenticationStore.currentToken}`;
  }
  return config;
}