/**
 * Subscription Plans API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "@/shared/services/http-common.js";

export class SubscriptionPlansApiService {
  constructor() {
    this.serviceBaseUrl = "/subscriptions/api/v1/plan";
  }
  async getAll() {
    const response = await http.get(`${this.serviceBaseUrl}`);
    return response.data;
  }
}