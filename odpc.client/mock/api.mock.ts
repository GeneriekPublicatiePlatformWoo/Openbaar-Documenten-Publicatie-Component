import { type MockHandler } from "vite-plugin-mock-server";
import type { Publicatie } from "@/features/publicatie/types";

const getIndex = (url: string | undefined) => +(url?.substring(url.lastIndexOf("/") + 1) || 0); // ...

const publicaties: Publicatie[] = [
  {
    uuid: "0",
    officieleTitel:
      "Openbaarheid en Verantwoording: De Impact van de Wet open overheid op Bestuurlijke Transparantie",
    verkorteTitel: "Openbaarheid en Verantwoording",
    omschrijving: "",
    creatiedatum: "2024-08-24"
  },
  {
    uuid: "1",
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
        const publicatie = {
          uuid: "" + publicaties.length,
          ...(JSON.parse(bodyString) as Publicatie)
        };

        publicaties.push(publicatie);

        setTimeout(() => res.end(JSON.stringify(publicatie)), 500);
      });
    }
  },
  {
    pattern: "/api-mock/publicaties/*",
    method: "GET",
    handle: (req, res) => {
      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(publicaties[getIndex(req.url)])), 500);
    }
  },
  {
    pattern: "/api-mock/publicaties/*",
    method: "PUT",
    handle: (req, res) => {
      res.setHeader("Content-Type", "application/json");

      req.on("data", (bodyString: string) => {
        const publicatie = JSON.parse(bodyString) as Publicatie;

        publicaties[getIndex(req.url)] = publicatie;

        setTimeout(() => res.end(JSON.stringify(publicatie)), 500);
      });
    }
  }
];

export default mocks;
