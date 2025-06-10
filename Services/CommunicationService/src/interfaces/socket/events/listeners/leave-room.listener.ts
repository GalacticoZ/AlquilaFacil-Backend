import { Socket } from "socket.io";

export function leaveRoomListener(socket: Socket) {
  socket.on("disconnect", () => {
    const roomId = socket.data.roomId;
    const userId = socket.data.userId;

    if (roomId && userId) {
      socket.to(roomId).emit("userLeft", { userId });
      console.log(`User ${userId} has left room ${roomId}`);
    }
  });
}
