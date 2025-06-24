// src/infrastructure/socket/events/index.ts

import { Socket } from "socket.io";
import { joinRoomListener } from "./listeners/join-room.listener";
import { sendMessageListener } from "./listeners/send-message.listener";
import { setupSignaling } from "../signaling/web-rtc.signaling";
import { checkRoomListener } from "./listeners/check-room.listener";
import { leaveRoomListener } from "./listeners/leave-room.listener";

/**
 * Function to handle socket events.
 * @param io - The Socket.IO server instance.
 */
export const handleEvents = (io: any) => {
  setupSignaling(io);
  io.on("connection", (socket: Socket) => {
    joinRoomListener(socket);
    checkRoomListener(socket, io);
    sendMessageListener(socket); 
    leaveRoomListener(socket );   
  });
};
