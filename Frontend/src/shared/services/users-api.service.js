/**
 * Users API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "@/shared/services/http-common.js";

export class UsersApiService {
  async getById(id) {
    const response = await http.get(`/users/${id}`);
    return response.data;
  }

  async update(id, userResource) {
    const response = await http.put(`/users/${id}`, userResource);
    return response.data;
  }

  async getUsernameById(id) {
    const response = await http.get(`/users/get-username/${id}`);
    return response.data;
  } 

}