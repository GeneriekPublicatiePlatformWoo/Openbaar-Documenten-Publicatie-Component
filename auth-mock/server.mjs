import Provider from "oidc-provider";

const host = process.env.HOST || "localhost";
const port = process.env.PORT || 3000;
const client_id = process.env.CLIENT_ID;
const client_secret = process.env.CLIENT_SECRET;

const redirect_uris = Object.entries(process.env)
  .filter(([key]) => key.startsWith("CLIENT_REDIRECT_URI"))
  .map(([, value]) => value);
const post_logout_redirect_uris = Object.entries(process.env)
  .filter(([key]) => key.startsWith("CLIENT_LOGOUT_REDIRECT_URI"))
  .map(([, value]) => value);

const oidc = new Provider(`http://${host}:${port}`, {
  features: {
    devInteractions: { enabled: true },
    registration: { enabled: false },
    revocation: { enabled: true },
  },
  async findAccount(ctx, id) {
    return {
      accountId: id,
      async claims(use, scope) {
        return { sub: id, roles: ["test"], name: id, preferred_username: id };
      },
    };
  },
  format: {
    default: "jwt",
    AccessToken: "jwt",
    RefreshToken: "jwt",
  },
  clients: [
    {
      client_id,
      client_secret,
      redirect_uris,
      post_logout_redirect_uris,
    },
  ],
});

let server;
try {
  server = oidc.listen(port, () => {
    console.log(
      `mock-oidc-user-server listening on port ${port}, check http://${host}:${port}/.well-known/openid-configuration`
    );
  });
} catch (error) {
  if (server && server.listening) server.close();
  console.error(err);
  process.exitCode = 1;
}
