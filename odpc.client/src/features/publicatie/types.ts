import type { RequiredPaginatedSearchParams } from "@/composables/use-paginated-search";

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
  creatiedatum: string;
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

type PublicatieSearchParams = Record<string, string> & RequiredPaginatedSearchParams;

export const publicatieSearchParams: PublicatieSearchParams = {
  page: "",
  sorteer: "",
  search: "",
  registratiedatum__gte: "",
  registratiedatum__lte: ""
};

export type PublicatieSearchParam = keyof typeof publicatieSearchParams;
