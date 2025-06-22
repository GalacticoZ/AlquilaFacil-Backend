export class SubscriptionPlanRequest {
  constructor({id, name, service, price}) {
    this.id = id;
    this.name = name;
    this.service = service;
    this.price = price;
  }
}