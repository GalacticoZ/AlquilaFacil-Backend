import { Message } from "../model/aggregates/message";
import { GetAllMessagesByRoomIdQuery } from "../model/queries/get-all-messages-by-room-id.query";

/**
 * @summary IMessageQueryService interface
 * @description This interface defines the contract for the Message query service, which is responsible for handling queries related to messages in a room.
 * @method handleGetAllMessagesByRoomIdQuery(getAllMessagesByRoomIdQuery: GetAllMessagesByRoomIdQuery): Promise<Message[]> - Handles the query to get all messages by room ID.
 */

export interface IMessageQueryService {
  handleGetAllMessagesByRoomIdQuery(getAllMessagesByRoomIdQuery: GetAllMessagesByRoomIdQuery): Promise<Message[]>;
}