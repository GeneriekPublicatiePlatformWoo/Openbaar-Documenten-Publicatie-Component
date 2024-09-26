FROM node:18-alpine
WORKDIR /app
COPY package.json .
RUN npm install oidc-provider
COPY server.mjs .
RUN chown -R node:node /app

USER 1000

CMD [ "npm", "run", "start" ]