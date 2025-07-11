/**
 * Locals API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "../../shared/services/http-common.js";
import { UsersApiService } from "../../shared/services/users-api.service.js";

export class LocalsApiService {

  constructor() {
    this.userApiService = new UsersApiService();
    this.serviceBaseUrl = "/locals/api/v1/locals";
  }

  async create(localResource) {
    const response = await http.post(`${this.serviceBaseUrl}`, localResource);
    return response.data;
  }

  async getAll() {
    const response = await http.get(`${this.serviceBaseUrl}`);
    return response.data;
  }

  async getById(id) {
    const response = await http.get(`${this.serviceBaseUrl}/${id}`);
    if(response.data) {
      const userUsername = await this.userApiService.getUsernameById(response.data.userId);
      return {
        ...response.data,
        features: response.data.features.split(','),
        userUsername: userUsername
      };
    }
  }

  async update(id, localResource) {
    const response = await http.put(`${this.serviceBaseUrl}/${id}`, localResource);
    return response.data;
  }

  async getByCategoryAndCapacityRange(categoryId, minCapacity, maxCapacity) {
    const response = await http.get(`${this.serviceBaseUrl}/by-category-and-capacity/${categoryId}/${minCapacity}/${maxCapacity}`);
    return response.data;
  }

  async getAllDistricts() {
    const response = await http.get(`${this.serviceBaseUrl}/districts`);
    return response.data;
  }

  async getUserLocals(userId) {
    const response = await http.get(`${this.serviceBaseUrl}/get-user-locals/${userId}`);
    return response.data;
  }

}