import mongoose from "mongoose";

export async function connectToDatabase() {
  try {
    await mongoose.connect("mongodb://communication-database:27017/chat");
    console.log("Connected to MongoDB");
  } catch (err) {
    console.error('Error', err)
  }
}