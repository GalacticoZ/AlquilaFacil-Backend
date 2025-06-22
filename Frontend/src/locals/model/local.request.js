export class LocalRequest {
  constructor({
    localName,
    descriptionMessage,
    country,
    city,
    district,
    street,
    price,
    capacity,
    photoUrls,
    features,
    localCategoryId,
    userId
  }) {
    this.localName = localName;
    this.descriptionMessage = descriptionMessage;
    this.country = country;
    this.city = city;
    this.district = district;
    this.street = street;
    this.price = parseFloat(price);
    this.capacity = parseInt(capacity);
    this.photoUrls = photoUrls;
    this.features = features.join(',');
    this.localCategoryId = localCategoryId;
    this.userId = userId;
    
  }
}