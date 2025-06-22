import http from "@/shared/services/http-common.js";
import { getAuth, GoogleAuthProvider, signInWithPopup } from "firebase/auth";

export class AuthenticationService {

  constructor() {
    this.serviceBaseUrl = "/iam/api/v1/authentication";
  }

  async signIn(signInRequest) {
    const response = await http.post(`${this.serviceBaseUrl}/sign-in`, signInRequest);
    return response.data;
  }

  async signUp(signUpRequest) {
    return await http.post(`${this.serviceBaseUrl}/sign-up`, signUpRequest);
  }

  async signInWithGoogle() {
    const provider = new GoogleAuthProvider();
    return signInWithPopup(getAuth(), provider)
      .then((result) => {
        return result.user;
      })
      .catch((error) => {
        console.error("Error signing in with Google: ", error);
        throw error;
      });
  }
}