import { RoomId } from "../valueobjects/room-id.value-object";
import { UserId } from "../valueobjects/user-id.value-object";

export class StartVideoCallCommand {
  constructor(
    public readonly initiatorId: UserId,
    public readonly roomId: RoomId,
  ) {}
}