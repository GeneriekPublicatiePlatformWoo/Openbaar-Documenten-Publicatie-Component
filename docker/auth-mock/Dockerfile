FROM node:lts-alpine3.19
WORKDIR /app
COPY package.json .
RUN npm install
COPY server.mjs .
RUN chown -R node:node /app

USER 1000

CMD [ "npm", "run", "start" ]