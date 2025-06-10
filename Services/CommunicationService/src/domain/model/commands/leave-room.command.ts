import { RoomId } from "../valueobjects/room-id.value-object";
import { UserId } from "../valueobjects/user-id.value-object";

export class LeaveRoomCommand {
  constructor(
    public readonly userId: UserId,
    public readonly roomId: RoomId,
  ) {}
}