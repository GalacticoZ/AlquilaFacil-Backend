import { Request, Response } from "express";
import { MessageRepository } from "../../../infrastructure/persistence/mongoose/repositories/message.repository";
import { MessageQueryService } from "../../../application/internal/query-services/message-query.service";
import { GetAllMessagesByRoomIdQuery } from "../../../domain/model/queries/get-all-messages-by-room-id.query";
import { RoomId } from "../../../domain/model/valueobjects/room-id.value-object";
import { MessageResourceFromEntityAssembler } from "../transform/message-resource-from-entity.assembler";
import { NotFoundError } from "../../../domain/model/errors/not-found.error";

const messageRepository = new MessageRepository();
const messageQueryService = new MessageQueryService(messageRepository);

export class MessagesController {
  async getMessagesByRoomId(req: Request, res: Response) {
    try {
      const roomId = new RoomId(req.params.roomId);
      const query = new GetAllMessagesByRoomIdQuery(roomId);
      const result = await messageQueryService.handleGetAllMessagesByRoomIdQuery(query);
      const messages = result.map(message => MessageResourceFromEntityAssembler.toResourceFromEntity(message));
      console.log("Messages retrieved:", messages);
      res.status(200).json(messages);
    } catch (error: any) {
      if (error instanceof NotFoundError) {
        res.status(404).json({ error: error.message });
      }
      else {
        res.status(500).json({ error: error.message });
      }
    }
  }
}