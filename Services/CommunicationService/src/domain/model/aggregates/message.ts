import { RoomId } from "../valueobjects/room-id.value-object";
import { UserId } from "../valueobjects/user-id.value-object";

/**
 * @summary Message aggregate root
 * @description This class represents a message in the system.
 */

export class Message {
  constructor(
    public readonly content: string,
    public readonly userId: UserId,
    public readonly roomId: RoomId,
    public readonly timestamp: Date,
    public readonly id?: number
  ) {}
}