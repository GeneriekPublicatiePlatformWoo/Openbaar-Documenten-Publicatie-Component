import { type MockHandler } from "vite-plugin-mock-server";

export const WAARDELIJSTEN = {
  ORGANISATIE: "Organisatie",
  INFORMATIECATEGORIE: "Informatiecategorie",
  THEMA: "Thema"
  // DOCUMENTSOORT: 'Documentsoort'
} as const;

export type Waardelijst = keyof typeof WAARDELIJSTEN;

export type WaardelijstItem = {
  id: string;
  type: Waardelijst;
  name: string;
  checked?: boolean;
};

export type GroupedWaardeLijstItems = {
  [key in Waardelijst]: WaardelijstItem[];
};

const mocks: MockHandler[] = [
  {
    pattern: "/api-mock/gebruikersgroep/*",
    method: "GET",
    handle: (_req, res) => {
      const data: number[] = [2, 3, 4, 5, 7];

      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(data)), 500);
    }
  },
  {
    pattern: "/api-mock/gebruikersgroep/*",
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
