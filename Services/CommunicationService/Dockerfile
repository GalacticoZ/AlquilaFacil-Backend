
FROM node:22.16.0-alpine AS builder

WORKDIR /app

COPY Services/CommunicationService/package*.json ./

RUN npm install

COPY Services/CommunicationService/ ./

RUN npm run tsc

FROM node:22.16.0-alpine

WORKDIR /app

COPY --from=builder /app/build ./build
COPY --from=builder /app/package*.json ./

RUN npm install --omit=dev

EXPOSE 8017

CMD ["npm", "start"]