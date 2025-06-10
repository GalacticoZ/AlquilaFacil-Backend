
import { SendMessageCommand } from "../../../domain/model/commands/send-message.command";
import { IMessageCommandService } from "../../../domain/services/message-command-service.interface";
import { IMessageRepository } from "../../../domain/repositories/message-repository.interface";
import { Message } from "../../../domain/model/aggregates/message";

export class MessageCommandService implements IMessageCommandService {
  constructor(private readonly messageRepository: IMessageRepository) {}

  async handleSendMessageCommand(sendMessageCommand: SendMessageCommand): Promise<void> {
    const message = new Message(
      sendMessageCommand.content,
      sendMessageCommand.userId,
      sendMessageCommand.roomId,
      sendMessageCommand.timestamp
    )
    await this.messageRepository.saveMessage(message);
  }
}
