import { SendMessageCommand } from "../model/commands/send-message.command";

/**
 * @summary IMessageCommandService interface
 * @description This interface defines the contract for the Message command service, which is responsible for handling commands related to sending messages in a room.
 * @method handleSendMessageCommand(sendMessageCommand: SendMessageCommand): Promise<void> - Handles the command to send a message in a room.
 */

export interface IMessageCommandService {
  handleSendMessageCommand(sendMessageCommand: SendMessageCommand): Promise<void>;
}