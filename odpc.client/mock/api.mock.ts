import { type MockHandler } from "vite-plugin-mock-server";
import { randomUUID } from "crypto";
import type { Publicatie, MimeTypes, PublicatieDocument } from "@/features/publicatie/types";

import publicatiesJson from "./publicaties.json";
import documentenJson from "./documenten.json";
import mimeTypesJson from "./mime-types.json";

const getIndex = (url: string | undefined) => +(url?.substring(url.lastIndexOf("/") + 1) || 0); // ...

const publicaties = publicatiesJson as Publicatie[];
const documenten = documentenJson as PublicatieDocument[];
const mimeTypes: MimeTypes[] = mimeTypesJson;

const mocks: MockHandler[] = [
  {
    pattern: "/api-mock/v1/publicaties",
    method: "GET",
    handle: (_req, res) => {
      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(publicaties)), 500);
    }
  },
  {
    pattern: "/api-mock/v1/publicaties",
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
    pattern: "/api-mock/v1/publicaties/*",
    method: "GET",
    handle: (req, res) => {
      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(publicaties[getIndex(req.url)])), 500);
    }
  },
  {
    pattern: "/api-mock/v1/publicaties/*",
    method: "PUT",
    handle: (req, res) => {
      res.setHeader("Content-Type", "application/json");

      req.on("data", (bodyString: string) => {
        const publicatie = JSON.parse(bodyString) as Publicatie;

        publicaties[getIndex(req.url)] = publicatie;

        setTimeout(() => res.end(JSON.stringify(publicatie)), 500);
      });
    }
  },
  {
    pattern: "/api-mock/v1/documenten/*",
    method: "GET",
    handle: (_req, res) => {
      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(documenten[0])), 500);
    }
  },
  {
    pattern: "/api-mock/v1/documenten/**",
    method: "POST",
    handle: (req, res) => {
      res.setHeader("Content-Type", "application/json");

      req.on("data", (bodyString: string) => {
        const body: any = JSON.parse(bodyString);
        const { bestandsomvang } = body;

        const uuid = randomUUID();
        const halve = bestandsomvang / 2;
        const firstSize = Math.ceil(halve);
        const secondSize = Math.floor(halve);

        const responseBody = {
          ...documenten[0],
          bestandsdelen: [
            {
              url: `/api-mock/v1/documenten/${uuid}/bestandsdelen/1`,
              volgnummer: 0,
              omvang: firstSize
            },
            {
              url: `/api-mock/v1/documenten/${uuid}/bestandsdelen/2`,
              volgnummer: 1,
              omvang: secondSize
            }
          ]
        };

        setTimeout(() => res.end(JSON.stringify(responseBody)), 500);
      });
    }
  },
  {
    pattern: "/api-mock/v1/documenten/*/bestandsdelen/*",
    method: "PUT",
    handle: (_req, res) => {
      setTimeout(() => res.end(), 500);
    }
  },
  {
    pattern: "/api-mock/v1/formats",
    method: "GET",
    handle: (_req, res) => {
      res.setHeader("Content-Type", "application/json");
      setTimeout(() => res.end(JSON.stringify(mimeTypes)), 500);
    }
  }
];

export default mocks;
