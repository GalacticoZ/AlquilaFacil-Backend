import express from "express";
import cors from "cors";
import swaggerUi from 'swagger-ui-express';
import swaggerSpec from './docs/swagger';
import { connectToDatabase } from "../../infrastructure/persistence/mongoose/database";

export async function createExpressApp() {
  try {
    await connectToDatabase();
    const app = express();
    app.use(express.json());

    app.use(
      cors({
        origin: '*', 
        credentials: true, 
        optionsSuccessStatus: 204 // For legacy browser support
      })
    );

    app.use('/swagger/index.html', swaggerUi.serve, swaggerUi.setup(swaggerSpec));

    //Routes
    app.use('/api/v1/messages', (await import('./routes/messages.routes')).default);
    return app;
  }
  catch (error) {
    console.error("Error creating Express app:", error);
    throw error;
  }
}
