import { SendMessageCommand } from "../../../domain/model/commands/send-message.command";
import { RoomId } from "../../../domain/model/valueobjects/room-id.value-object";
import { UserId } from "../../../domain/model/valueobjects/user-id.value-object";
import { SendMessageCommandResource } from "../resources/send-message-command.resource";

export class SendMessageCommandFromResourceAssembler {
  /**
   * @summary Converts a SendMessageCommandResource to a SendMessageCommand.
   * @param resource - The SendMessageCommandResource to convert.
   * @returns A SendMessageCommand instance.
   */
  
  public static toCommandFromResource(resource: SendMessageCommandResource): SendMessageCommand {
    return new SendMessageCommand(
      resource.content,
      new UserId(resource.userId),
      new RoomId(resource.roomId),
      new Date()
    );
  }
}