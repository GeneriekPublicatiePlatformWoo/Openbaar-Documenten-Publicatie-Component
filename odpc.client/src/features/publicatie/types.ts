export const PUBLICATIESTATUS = {
  draft: "Concept",
  published: "Gepubliceerd",
  retracted: "Ingetrokken"
} as const;

export type Publicatie = {
  uuid?: string;
  officieleTitel: string;
  verkorteTitel: string;
  omschrijving: string;
  creatiedatum: string;
  status?: keyof typeof PUBLICATIESTATUS;
};

export type PublicatieDocument = {
  uuid?: string;
  publicatie: string;
  officieleTitel: string;
  verkorteTitel?: string;
  omschrijving?: string;
  creatiedatum: string;
  bestandsnaam: string;
  bestandsformaat?: string;
  bestandsomvang: number;
};

export type MimeTypes = {
  identifier: string;
  name: string;
  mimeType: string;
};
