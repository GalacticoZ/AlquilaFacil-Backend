import mongoose from "mongoose";

const MessageSchema = new mongoose.Schema({
  content: { type: String, required: true },
  userId: { type: Number, required: true },
  roomId: { type: String, required: true },
  timestamp: { type: Date, required: true }
});


export const MessageModel = mongoose.model("Message", MessageSchema);