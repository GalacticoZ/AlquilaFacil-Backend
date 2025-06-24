/**
 * Notifications API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "@/shared/services/http-common.js";

export class NotificationsApiService {

  constructor() {
    this.serviceBaseUrl = "/notification/api/v1/notification";
  }

  async getByUserId(userId) {
    const response = await http.get(`${this.serviceBaseUrl}/by-user-id/${userId}`);
    return response.data;
  }
}