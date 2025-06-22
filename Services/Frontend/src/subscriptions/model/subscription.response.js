export class SubscriptionResponse {
  constructor({id, userId, planId, subscriptionStatusId}) {
    this.id = id;
    this.userId = userId;
    this.planId = planId;
    this.subscriptionStatusId = subscriptionStatusId;
  }
}