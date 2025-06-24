/**
 * Subscriptions API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "../../shared/services/http-common.js";

export class SubscriptionsApiService {
  constructor() {
    this.serviceBaseUrl = "/subscriptions/api/v1/subscriptions";
  }
  async create(subscriptionResource) {
    const response = await http.post(`${this.serviceBaseUrl}`, subscriptionResource);
    return response.data;
  }
  async getAll() {
    const response = await http.get(`${this.serviceBaseUrl}`);
    console.log(response.data);
    return response.data;
  }
  async getById(id) {
    const response = await http.get(`${this.serviceBaseUrl}/${id}`);
    return response.data;
  }
  async activeSubscriptionStatus(subscriptionId) {
    const response = await http.put(`${this.serviceBaseUrl}/${subscriptionId}`);
    return response.data;
  }
}