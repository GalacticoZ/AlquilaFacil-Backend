import { Room } from "../../../../domain/model/aggregates/room";
import { RoomId } from "../../../../domain/model/valueobjects/room-id.value-object";
import { UserId } from "../../../../domain/model/valueobjects/user-id.value-object";
import { IRoomRepository } from "../../../../domain/repositories/room-repository.interface";
import { RoomModel } from "../schemas/room.schema";

export class RoomRepository implements IRoomRepository {
  async createRoom(userIds: UserId[]): Promise<Room> {
    try {
      console.log("Creating room with userIds:", userIds.map(u => u.value));
      const model = new RoomModel({
        userIds: userIds.map(userId => userId.value),
      });
      const room = await model.save(); 
      return new Room(
        new RoomId(room._id!.toString()),
        room.userIds.map((id: number) => new UserId(id))
      ); 
    } catch (err) {
      console.error("Error while creating room:", err);
      throw err;
    }
  }

  async findByUserIds(userIds: UserId[]): Promise<Room | null> {
    try {
      const userIdValues = userIds.map(u => u.value);
      const room = await RoomModel.findOne({
        userIds: { $all: userIdValues, $size: userIdValues.length }
      }).lean();
      if (!room) return null;
      return new Room(
        new RoomId(room._id!.toString()),
        room.userIds.map((id: number) => new UserId(id))
      );
    } catch (err) {
      console.error("Error while finding room by userIds:", err);
      throw err;
    }    
  }
}