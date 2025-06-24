import { Server } from "socket.io";
import { Server as HttpServer } from "http"; 
import { handleEvents } from "./events";
import { setupSignaling } from "./signaling/web-rtc.signaling";

/**
 * @param httpServer - The HTTP server instance to attach the Socket.IO server to.
 * @returns {Server} - The Socket.IO server instance.
 */

export function createSocketServer(httpServer: HttpServer): Server {
  const io = new Server(httpServer, {
    cors: {
      origin: "*",
    },
  });

  handleEvents(io);
  setupSignaling(io);

  return io;
}
