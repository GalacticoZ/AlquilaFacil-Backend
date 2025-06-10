import { Message } from "../model/aggregates/message";
import { RoomId } from "../model/valueobjects/room-id.value-object";

export interface IMessageRepository {
  saveMessage(message: Message): Promise<void>;
  getMessagesByRoomId(roomId: RoomId): Promise<Message[]>;
}