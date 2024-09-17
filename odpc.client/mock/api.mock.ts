import { type MockHandler } from "vite-plugin-mock-server";
import type { Publicatie } from "@/features/publicatie/types";

export const publicaties: Publicatie[] = [
  {
    identifier: "a0-b1",
    officieleTitel: "Openbaarheid en Verantwoording: De Impact van de Wet open overheid op Bestuurlijke Transparantie",
    verkorteTitel: "Openbaarheid en Verantwoording",
    omschrijving: "",
    creatiedatum: "2024-08-24"
  },
  {
    identifier: "c2-d3",
    officieleTitel: "Inzicht voor Iedereen: Toepassing en Resultaten van de Wet open overheid",
    verkorteTitel: "Inzicht voor Iedereen",
    omschrijving: "",
    creatiedatum: "2024-05-02"
  }
];

const mocks: MockHandler[] = [
  {
    pattern: "/api-mock/publicaties",
    method: "GET",
    handle: (_req, res) => {
      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(publicaties)), 500);
    }
  },
  {
    pattern: "/api-mock/publicaties",
    method: "POST",
    handle: (req, res) => {
      res.setHeader("Content-Type", "application/json");

      req.on("data", (bodyString: string) => {
        const body: object = JSON.parse(bodyString);

        setTimeout(() => res.end(JSON.stringify({ identifier: "a0-b1", ...body })), 500);
      });
    }
  },
  {
    pattern: "/api-mock/publicaties/*",
    method: "GET",
    handle: (req, res) => {
      const index = +!req.url?.includes("a0-b1");

      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(publicaties[index])), 500);
    }
  },
  {
    pattern: "/api-mock/publicaties/*",
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
