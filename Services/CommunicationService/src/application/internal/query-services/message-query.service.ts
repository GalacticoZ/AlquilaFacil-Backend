import { Message } from "../../../domain/model/aggregates/message";
import { NotFoundError } from "../../../domain/model/errors/not-found.error";
import { GetAllMessagesByRoomIdQuery } from "../../../domain/model/queries/get-all-messages-by-room-id.query";
import { IMessageRepository } from "../../../domain/repositories/message-repository.interface";
import { IMessageQueryService } from "../../../domain/services/message-query-service.interface";

export class MessageQueryService implements IMessageQueryService {
  constructor(private readonly messageRepository: IMessageRepository) {}

  async handleGetAllMessagesByRoomIdQuery(getAllMessagesByRoomIdQuery: GetAllMessagesByRoomIdQuery): Promise<Message[]> {
    const { roomId } = getAllMessagesByRoomIdQuery;
    const messages = await this.messageRepository.getMessagesByRoomId(roomId);
    if (!messages || messages.length === 0) {
      throw new NotFoundError(`No messages found for room ID: ${roomId.value}`);
    }
    return messages;
  }
}