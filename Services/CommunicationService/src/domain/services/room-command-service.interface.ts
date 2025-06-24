import { Room } from "../model/aggregates/room";
import { CreateRoomCommand } from "../model/commands/create-room.command";
import { JoinRoomCommand } from "../model/commands/join-room.command";
import { RoomId } from "../model/valueobjects/room-id.value-object";

/**
 * @summary IRoomCommandService interface
 * @description This interface defines the contract for the Room command service, which is responsible for handling commands related to Room aggregates.
 * @method handleCreateRoomCommand(createRoomCommand: CreateRoomCommand): Promise<void> - Handles the command to create a new room.
 * @method handleJoinRoomCommand(joinRoomCommand: JoinRoomCommand): Promise<void> - Handles the command to join a room.
 * @method handle(leaveRoomCommand: LeaveRoomCommand): Promise<void> - Handles the command to leave a room.
 * @method handle(startVideoCallCommand: StartVideoCallCommand): Promise<void> - Handles the command to start a video call in a room.
 */

export interface IRoomCommandService {
  handleCreateRoomCommand(createRoomCommand: CreateRoomCommand): Promise<Room>;
  handleJoinRoomCommand(joinRoomCommand: JoinRoomCommand): Promise<RoomId>;
}