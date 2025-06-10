import { UserId } from "../valueobjects/user-id.value-object";

export class CreateRoomCommand {
  constructor(
    public readonly userIds: UserId[]
  ) {}
}