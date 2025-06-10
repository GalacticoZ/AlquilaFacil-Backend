export class SendMessageCommandResource {
  constructor(
    public readonly content: string,
    public readonly userId: number,
    public readonly roomId: string
  ) {}
}