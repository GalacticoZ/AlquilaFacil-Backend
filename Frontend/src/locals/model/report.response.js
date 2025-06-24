export class ReportResponse {
  constructor({id, localId, title, userId, description}) {
    this.id = id;
    this.localId = localId;
    this.title = title;
    this.userId = userId;
    this.description = description;
  }
}