import { type MockHandler } from "vite-plugin-mock-server";

export const WAARDELIJSTEN = {
  ORGANISATIE: "Organisatie",
  THEMA: "Thema",
  INFORMATIECATEGORIE: "Informatiecategorie"
  // DOCUMENTSOORT: 'Documentsoort'
} as const;

export type Waardelijst = keyof typeof WAARDELIJSTEN;

export type WaardelijstItem = {
  id: number;
  type: Waardelijst;
  name: string;
  checked?: boolean;
};

export type GroupedWaardeLijstItems = {
  [key in Waardelijst]: WaardelijstItem[];
};

const mocks: MockHandler[] = [
  {
    pattern: "/api/waardelijsten",
    handle: (_req, res) => {
      const data: WaardelijstItem[] = [
        {
          id: 6,
          type: "THEMA",
          name: "arbeidsomstandigheden"
        },
        {
          id: 8,
          type: "INFORMATIECATEGORIE",
          name: "wet of algemeen bindend voorschrift"
        },
        {
          id: 5,
          type: "THEMA",
          name: "afval"
        },
        {
          id: 2,
          type: "ORGANISATIE",
          name: "Organisatie 2"
        },
        {
          id: 3,
          type: "ORGANISATIE",
          name: "Organisatie 3"
        },
        {
          id: 4,
          type: "THEMA",
          name: "europese zaken"
        },
        {
          id: 7,
          type: "THEMA",
          name: "arbeidsongeschikheid"
        },
        {
          id: 1,
          type: "ORGANISATIE",
          name: "Organisatie 1"
        }
      ];

      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(data)), 500);
    }
  },
  {
    pattern: "/api/gebruikersgroep/*",
    method: "GET",
    handle: (_req, res) => {
      const data: number[] = [2, 3, 4, 5, 7];

      res.setHeader("Content-Type", "application/json");

      setTimeout(() => res.end(JSON.stringify(data)), 500);
    }
  },
  {
    pattern: "/api/gebruikersgroep/*",
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
