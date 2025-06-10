import { Router } from "express";
import { MessagesController } from "../controllers/messages.controller";

const router = Router();
const messageController = new MessagesController();

/**
 * @swagger
 * /api/v1/messages/{roomId}:
 *   get:
 *     summary: Retrieve messages by room ID
 *     description: Fetches all messages associated with a specific room ID.
 *     tags:
 *       - Messages
 *     parameters:
 *       - in: path
 *         name: roomId
 *         required: true
 *         description: The ID of the room to retrieve messages for.
 *         schema:
 *           type: string
 *     responses:
 *       200:
 *         description: A list of messages for the specified room ID.
 *         content:
 *           application/json:
 *             schema:
 *               type: array
 *               items:
 *                 type: object
 *                 properties:
 *                   content:
 *                     type: string
 *                   userId:
 *                     type: integer
 *                   roomId:
 *                     type: string
 *                   timestamp:
 *                     type: string
 *                     format: date-time
 *       404:
 *         description: Room not found or no messages available for the specified room ID.
 *         content:
 *           application/json:
 *             schema:
 *               type: string
 *       500:
 *         description: Internal server error.
 *         content:
 *           application/json:
 *             schema:
 *               type: string
 */

router.get("/:roomId", messageController.getMessagesByRoomId.bind(messageController));

export default router;
