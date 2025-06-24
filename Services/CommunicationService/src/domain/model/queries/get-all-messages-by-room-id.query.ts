import { RoomId } from "./../valueobjects/room-id.value-object";

export class GetAllMessagesByRoomIdQuery {
  constructor(
    public readonly roomId: RoomId
  ) {}
}