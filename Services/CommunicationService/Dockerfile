FROM node:22.16.0-alpine

RUN mkdir -p /usr/src/app

WORKDIR /usr/src/app

COPY Services/CommunicationService/package.json ./

RUN npm install

COPY Services/CommunicationService/ ./

EXPOSE 8017

CMD ["npm", "run", "dev"]