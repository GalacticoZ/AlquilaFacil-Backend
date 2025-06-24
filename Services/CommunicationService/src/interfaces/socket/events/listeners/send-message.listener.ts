import { Socket } from "socket.io";
import { MessageCommandService } from "../../../../application/internal/command-services/message-command.service";
import { MessageRepository } from "../../../../infrastructure/persistence/mongoose/repositories/message.repository";
import { SendMessageCommandFromResourceAssembler } from "../../transform/send-message-command-from-resource.assembler";
import { SendMessageCommandResource } from "../../resources/send-message-command.resource";

const messageRepository = new MessageRepository();
const messageCommandService = new MessageCommandService(messageRepository);

export function sendMessageListener(socket: Socket) {
  socket.on("sendMessage", async (data: { content: string, userId: number, roomId: string }) => {
    try {
      const resource = new SendMessageCommandResource(data.content, data.userId, data.roomId);
      const command = SendMessageCommandFromResourceAssembler.toCommandFromResource(resource)
      await messageCommandService.handleSendMessageCommand(command);
      socket.to(data.roomId).emit("message", {
        content: data.content,
        userId: data.userId,
        timestamp: new Date()
      });
    } catch (err) {
      console.error("Error al manejar el mensaje:", err);
      socket.emit("error", { message: "No se pudo enviar el mensaje." });
    }
  });
}
