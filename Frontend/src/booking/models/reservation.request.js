export class ReservationRequest {
  constructor({ startDate, endDate, userId, localId, price, voucherImageUrl }) {
    this.startDate = startDate;
    this.endDate = endDate;
    this.userId = userId;
    this.localId = localId;
    this.price = price;
    this.voucherImageUrl = voucherImageUrl;
  }
}