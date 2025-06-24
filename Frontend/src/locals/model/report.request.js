export class ReportRequest {
  constructor({localId, title, userId, description}) {
    this.localId = localId;
    this.title = title;
    this.userId = userId;
    this.description = description;
  }
}