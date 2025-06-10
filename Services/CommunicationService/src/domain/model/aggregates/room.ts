import { RoomId } from "../valueobjects/room-id.value-object";
import { UserId } from "../valueobjects/user-id.value-object";

/**
 * @summary Room aggregate root
 * @description This class represents a room in the system, which can have multiple participants and an active video call.
 */

export class Room {
  constructor(
    public readonly id: RoomId,
    public readonly usersIds: UserId[],
  ) {}
}