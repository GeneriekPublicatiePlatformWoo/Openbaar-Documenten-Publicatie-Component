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
};

export const Documenthandelingen = Object.freeze({
  ondertekening: "ondertekening",
  ontvangst: "ontvangst",
  vaststelling: "vaststelling"
});

export type Documenthandeling = {
  soortHandeling: keyof typeof Documenthandelingen;
  tijdstip: string;
};

export type PublicatieDocument = {
  uuid?: string;
  publicatie: string;
  officieleTitel: string;
  documenthandeling: Documenthandeling;
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
