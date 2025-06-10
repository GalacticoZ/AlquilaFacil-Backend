import mongoose from "mongoose";

const roomSchema = new mongoose.Schema({
  userIds: { type: [Number], required: true },
  createdAt: { type: Date, default: Date.now },
});

export const RoomModel = mongoose.model("Room", roomSchema);