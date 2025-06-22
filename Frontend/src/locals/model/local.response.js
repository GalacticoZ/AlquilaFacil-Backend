export class LocalResponse {
  constructor({
    id,
    localName,
    descriptionMessage,
    address,
    price,
    capacity,
    photoUrls,
    features,
    localCategoryId,
    userId,
    userUsername
  }) {
    this.id = id;
    this.localName = localName;
    this.descriptionMessage = descriptionMessage;
    this.address = address;
    this.price = price;
    this.capacity = capacity;
    this.photoUrls = photoUrls;
    this.features = features;
    this.localCategoryId = localCategoryId;
    this.userId = userId;
    this.userUsername = userUsername;
  }
}