/**
 * Reports API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "../../shared/services/http-common.js";

export class ReportsApiService {

  constructor() {
    this.serviceBaseUrl = "/report";
  }

  async create(reportResource) {
    const response = await http.post(`${this.serviceBaseUrl}`, reportResource);
  }
  async getByUserId(userId) {
    const response = await http.get(`${this.serviceBaseUrl}/by-user-id/${userId}`);
    return response.data;
  }
  async getByLocalId(localId) {
    const response = await http.get(`${this.serviceBaseUrl}/by-local-id/${localId}`);
    return response.data;
  }
  async delete(reportId) {
    const response = await http.delete(`${this.serviceBaseUrl}/${reportId}`);
    return response.data;
  }
}