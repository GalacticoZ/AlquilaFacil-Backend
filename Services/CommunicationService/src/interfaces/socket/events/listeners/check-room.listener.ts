import { Socket, Server } from "socket.io";

/**
 * Listener for checking if all users are connected in a room.
 * @param socket - The Socket.IO socket instance.
 * @param io - The Socket.IO server instance.
 */

export function checkRoomListener(socket: Socket, io: Server) {
  socket.on("checkRoom", async (data: { roomId: string }) => {
    try {
      const roomId: string = data.roomId;
      const socketsInRoom = await io.in(roomId).fetchSockets();
      const connectedUsersCount: number = socketsInRoom.length;
      const isEveryoneConnected: boolean = connectedUsersCount == 2;

      socket.emit("checkRoom", { isEveryoneConnected });
    } catch (error: any) {
      socket.emit("error", { message: error.message });
    }
  });
}
