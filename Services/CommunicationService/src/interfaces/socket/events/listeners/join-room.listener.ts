import { Socket } from "socket.io";
import { JoinRoomCommand } from "../../../../domain/model/commands/join-room.command";
import { RoomCommandService } from "../../../../application/internal/command-services/room-command.service";
import { RoomRepository } from "../../../../infrastructure/persistence/mongoose/repositories/room.repository";
import { UserId } from "../../../../domain/model/valueobjects/user-id.value-object";
import { RoomId } from "../../../../domain/model/valueobjects/room-id.value-object";

const roomRepository = new RoomRepository();
const roomCommandService = new RoomCommandService(roomRepository);

export function joinRoomListener(socket: Socket) {
  socket.on("joinRoom", async (data: { userId: number, userIds: number[],  }) => {
    try {
      const userIds = data.userIds.map(userId => new UserId(userId));
      const command = new JoinRoomCommand(userIds);
      const joinedRoomId: RoomId = await roomCommandService.handleJoinRoomCommand(command);
      const roomId: string = joinedRoomId.value.toString(); // Convert RoomId to string to use as room identifier
      socket.join(roomId);
      socket.data.roomId = roomId;
      socket.data.userId = data.userId;
      socket.to(roomId).emit("userJoined", { userId: data.userId });
      socket.emit("joinedRoom", { roomId });
    } catch (error: any) {
      socket.emit("error", { message: error.message });
    }
  });
}
