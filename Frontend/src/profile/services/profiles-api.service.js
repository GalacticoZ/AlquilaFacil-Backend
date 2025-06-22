/**
 * Profiles API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "../../shared/services/http-common.js";

export class ProfilesApiService {

  constructor() {
    this.serviceBaseUrl = "/profiles/api/v1/profiles";
  }
  async getByUserId(userId) {
    const response = await http.get(`${this.serviceBaseUrl}/user/${userId}`);
    return response.data;
  } 
  async update(id, profileResource) {
    const response = await http.put(`${this.serviceBaseUrl}/${id}`, profileResource);
    return response.data;
  }
  async getSubscriptionStatusByUserId(userId) {
    const response = await http.get(`${this.serviceBaseUrl}/subscription-status/${userId}`);
    return response.data;
  }
  async getBankAccountsByUserId(userId) {
    const response = await http.get(`${this.serviceBaseUrl}/bank-accounts/${userId}`);
    const [bankAccountNumber, interbankAccountNumber] = response.data;
    return { bankAccountNumber, interbankAccountNumber };
  }
}