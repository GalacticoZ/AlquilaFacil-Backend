import { UserId } from "../valueobjects/user-id.value-object";

export class JoinRoomCommand {
  constructor(
    public readonly userIds: UserId[]
  ) {}
}