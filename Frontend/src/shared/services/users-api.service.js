/**
 * Users API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "@/shared/services/http-common.js";

export class UsersApiService {
  constructor() {
      this.serviceBaseUrl = "/iam/api/v1/users";
  }  
  async getById(id) {
    const response = await http.get(`${this.serviceBaseUrl}/${id}`);
    return response.data;
  }

  async update(id, userResource) {
    const response = await http.put(`${this.serviceBaseUrl}/${id}`, userResource);
    return response.data;
  }

  async getUsernameById(id) {
    const response = await http.get(`${this.serviceBaseUrl}/get-username/${id}`);
    return response.data;
  } 

}