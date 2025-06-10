import { UserId } from "../model/valueobjects/user-id.value-object";
import { Room } from "../model/aggregates/room";

/**
 * @summary IRoomRepository interface
 * @description This interface defines the contract for the Room repository, which is responsible for managing Room aggregates.
 * @method createRoom(userIds: UserId[]): Promise<void> - Creates a new room in the repository with the given user IDs.
 * @method findByUserIds(userIds: UserId[]): Promise<Room | null> - Finds a room by its ID.
 */

export interface IRoomRepository {
  createRoom(userIds: UserId[]): Promise<Room>;
  findByUserIds(userIds: UserId[]): Promise<Room | null>;
}