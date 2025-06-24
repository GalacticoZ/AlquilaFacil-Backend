export class CommentRequest {
  constructor({userId, localId, text, rating}) {
    this.userId = userId;
    this.localId = localId;
    this.text = text;
    this.rating = rating;
  }
}