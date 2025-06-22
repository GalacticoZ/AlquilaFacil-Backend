export class SubscriptionRequest {
  constructor({planId, userId, voucherImageUrl}) {
    this.planId = planId;
    this.userId = userId;
    this.voucherImageUrl = voucherImageUrl;
  }
}