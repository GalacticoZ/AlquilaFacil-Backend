import { Socket } from "socket.io";

export const setupSignaling = (io: any) => {
  io.on("connection", (socket: Socket) => {
    // WebRTC Offer: when a user wants to start a WebRTC connection
    socket.on("webrtc_offer", ({ roomId, offer, senderId }) => {
      socket.to(roomId).emit("webrtc_offer", { offer, senderId });
    });

    // WebRTC Answer: when the other user responds to the offer
    socket.on("webrtc_answer", ({ roomId, answer, senderId }) => {
      socket.to(roomId).emit("webrtc_answer", { answer, senderId });
    });

    // ICE Candidate: when a user sends an ICE candidate to establish the connection
    socket.on("ice_candidate", ({ roomId, candidate, senderId }) => {
      socket.to(roomId).emit("ice_candidate", { candidate, senderId });
    });
  });
};
