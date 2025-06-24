/**
 * Reservations API Service Client
 * @description This class includes methods for typical REST resource operations.
 */
import http from "@/shared/services/http-common.js";
import { formatReservationDay } from "../utils/formatReservationDay";

export class ReservationsApiService {

  constructor() {
    this.serviceBaseUrl = "/booking/api/v1/reservation";
  }

  async create(reservationResource) {
    const response = await http.post(`${this.serviceBaseUrl}`, reservationResource);
    return response.data;
  }
  async update(id, reservationResource) {
    const response = await http.put(`${this.serviceBaseUrl}/${id}`, reservationResource);
    return response.data;
  }
  async delete(id) {
    const response = await http.delete(`${this.serviceBaseUrl}/${id}`);
    return response.data;
  }
  async getByUserId(userId) {
    const response = await http.get(`${this.serviceBaseUrl}/by-user-id/${userId}`);
    let reservations;
    if(response.data) {
      console.log(response.data)
      reservations = response.data.map((reservation) => {
        return {
          ...reservation,
          start: formatReservationDay(reservation.startDate),
          end: formatReservationDay(reservation.endDate),
          title: 'Reserva de un local',
          calendarId: 'userReservation' 
        };
      })
    }
    return reservations;
  }
  async getByOwnerId(ownerId) {
    try {
      const response = await http.get(`${this.serviceBaseUrl}/reservation-user-details/${ownerId}`);
      
      if (typeof response.data !== 'string') {
        const reservations = response.data.reservations.map((reservation) => {
          return {
            ...reservation,
            start: formatReservationDay(reservation.startDate),
            end: formatReservationDay(reservation.endDate),
            title: reservation.isSubscribe
              ? 'Reserva de tu espacio por usuario premium'
              : 'Reserva de tu espacio',
            calendarId: reservation.isSubscribe
              ? 'premiumUserLocalReservation'
              : 'localReservation',
          };
        });
        return reservations;
      }
  
      // En caso de que `response.data` sea un string, igual devuelve array vacío
      return [];
    } catch (error) {
      // Si ocurre un error (como un 404), devuelve también array vacío
      return [];
    }
  }
}