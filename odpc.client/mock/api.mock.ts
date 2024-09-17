import { type MockHandler } from "vite-plugin-mock-server";

const mocks: MockHandler[] = [
  {
    pattern: "/api-mock/*",
    method: "GET",
    handle: (_req, res) => {
      const data: number[] = [2, 3, 4, 5, 7];

      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(data)), 500);
    }
  },
  {
    pattern: "/api-mock/*",
    method: "PUT",
    handle: (req, res) => {
      res.setHeader("Content-Type", "application/json");

      req.on("data", (bodyString: string) => {
        const body: object = JSON.parse(bodyString);

        setTimeout(() => res.end(JSON.stringify(body)), 500);
      });
    }
  }
];

export default mocks;
