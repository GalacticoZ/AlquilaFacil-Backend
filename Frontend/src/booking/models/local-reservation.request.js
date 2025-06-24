export class LocalReservationRequest {
  constructor({ id, startDate, endDate, userId, localId, isSuscribed, voucherImageUrl }) {
    this.id = id;
    this.startDate = startDate;
    this.endDate = endDate;
    this.userId = userId;
    this.localId = localId;
    this.isSuscribed = isSuscribed;
    this.voucherImageUrl = voucherImageUrl
  }
}