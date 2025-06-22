export class CommentResponse {
  constructor({id, userId, userUsername, localId, text, rating}) {
    this.id = id;
    this.userId = userId;
    this.userUsername = userUsername;
    this.localId = localId;
    this.text = text;
    this.rating = rating;
  }
}