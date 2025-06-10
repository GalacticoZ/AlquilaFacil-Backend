import { RoomId } from "../valueobjects/room-id.value-object";
import { UserId } from "../valueobjects/user-id.value-object";

export class SendMessageCommand {
  constructor(
    public readonly content: string,
    public readonly userId: UserId,
    public readonly roomId: RoomId,
    public readonly timestamp: Date
  ) {}
}