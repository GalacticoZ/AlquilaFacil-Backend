/**
 * Local Categories API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "../../shared/services/http-common.js";

export class LocalCategoriesApiService {

  constructor() {
    this.serviceBaseUrl = "/locals/api/v1/local-categories";
  }

  async getAll() {
    const response = await http.get(`${this.serviceBaseUrl}`);
    return response.data;
  }
}