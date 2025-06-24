import { createServer } from "http";
import { createExpressApp } from "./interfaces/rest/express-server";
import { createSocketServer } from "./interfaces/socket/socket-server";

async function main() {
  const app = await createExpressApp();
  const httpServer = createServer(app);

  createSocketServer(httpServer);

  httpServer.listen(8017, () => {
    console.log("Server running on http://localhost:8017");
  });
}

main();