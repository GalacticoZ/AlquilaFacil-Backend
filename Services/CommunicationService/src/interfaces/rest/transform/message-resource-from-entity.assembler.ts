import { Message } from "../../../domain/model/aggregates/message";
import { MessageResource } from "./../resources/message.resource";

export class MessageResourceFromEntityAssembler {
  /**
   * @summary Converts a Message entity to a MessageResource.
   * @param message - The Message entity to convert.
   * @returns A MessageResource instance.
   */
  public static toResourceFromEntity(message: Message): MessageResource {
    return new MessageResource(
      message.content,
      message.userId.value,
      message.roomId.value,
      message.timestamp
    );
  }
}