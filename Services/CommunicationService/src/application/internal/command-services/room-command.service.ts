import { Room } from "../../../domain/model/aggregates/room";
import { CreateRoomCommand } from "../../../domain/model/commands/create-room.command";
import { JoinRoomCommand } from "../../../domain/model/commands/join-room.command";
import { RoomId } from "../../../domain/model/valueobjects/room-id.value-object";
import { IRoomRepository } from "../../../domain/repositories/room-repository.interface";
import { IRoomCommandService } from "../../../domain/services/room-command-service.interface";

export class RoomCommandService implements IRoomCommandService {
  constructor(private readonly roomRepository: IRoomRepository) {}

  async handleCreateRoomCommand(createRoomCommand: CreateRoomCommand): Promise<Room> {
    console.log("Handling CreateRoomCommand with userIds:", createRoomCommand.userIds);
    const { userIds } = createRoomCommand;
    const room = await this.roomRepository.createRoom(userIds);
    return room;
  }

  async handleJoinRoomCommand(joinRoomCommand: JoinRoomCommand): Promise<RoomId> {
    const { userIds } = joinRoomCommand;
    const room = await this.roomRepository.findByUserIds(userIds);
    if(!room) {
      const newRoom = await this.handleCreateRoomCommand(new CreateRoomCommand(userIds));
      return newRoom.id;
    }
    return room.id;
  }
}
