export const PublicatieStatus = Object.freeze({
  concept: "concept",
  gepubliceerd: "gepubliceerd",
  ingetrokken: "ingetrokken"
});

export type Publicatie = {
  uuid?: string;
  officieleTitel: string;
  verkorteTitel: string;
  omschrijving: string;
  registratiedatum: string;
  status?: keyof typeof PublicatieStatus;
};

export type PublicatieDocument = {
  uuid?: string;
  publicatie: string;
  officieleTitel: string;
  verkorteTitel?: string;
  omschrijving?: string;
  creatiedatum: string;
  bestandsnaam: string;
  bestandsformaat: string;
  bestandsomvang: number;
  bestandsdelen?: Bestandsdeel[];
  status: keyof typeof PublicatieStatus;
};

export type Bestandsdeel = {
  url: string;
  volgnummer: number;
  omvang: number;
};

export type MimeTypes = {
  identifier: string;
  name: string;
  mimeType: string;
};

export const publicatieSearchParams = {
  page: "1",
  sorteer: "-registratiedatum",
  search: "",
  registratiedatum__gte: "",
  registratiedatum__lte: ""
};

export type PublicatieSearchParam = keyof typeof publicatieSearchParams;

export const publicatieSortingOptions = {
  officiele_titel: "Title (a-z)",
  "-officiele_titel": "Title (z-a)",
  verkorte_titel: "Verkorte title (a-z)",
  "-verkorte_titel": "Verkorte title (z-a)",
  registratiedatum: "Registratiedatum (oud-nieuw)",
  "-registratiedatum": "Registratiedatum (nieuw-oud)"
};
