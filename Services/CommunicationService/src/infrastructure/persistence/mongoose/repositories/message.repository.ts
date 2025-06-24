import { Message } from "../../../../domain/model/aggregates/message";
import { RoomId } from "../../../../domain/model/valueobjects/room-id.value-object";
import { UserId } from "../../../../domain/model/valueobjects/user-id.value-object";
import { IMessageRepository } from "../../../../domain/repositories/message-repository.interface";
import { MessageModel } from "../schemas/message.schema";

export class MessageRepository implements IMessageRepository {
  async saveMessage(message: Message): Promise<void> {
    const model = new MessageModel({
      content: message.content,
      userId: message.userId.value,
      roomId: message.roomId.value,
      timestamp: message.timestamp
    })

    await model.save();
  }

  async getMessagesByRoomId(roomId: RoomId): Promise<Message[]> {
    const messages = await MessageModel.find({ roomId: roomId.value }).lean();

    return messages.map(message => new Message(
      message.content,
      new UserId(message.userId),
      new RoomId(message.roomId),
      message.timestamp
    ));
  }
  
}