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
  status: keyof typeof PublicatieStatus;
  informatieCategorieen: string[];
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
