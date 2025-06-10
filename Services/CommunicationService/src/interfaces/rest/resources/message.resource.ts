export class MessageResource {
  constructor(
    public readonly content: string,
    public readonly userId: number,
    public readonly roomId: string,
    public readonly timestamp: Date,
  ) {}
}