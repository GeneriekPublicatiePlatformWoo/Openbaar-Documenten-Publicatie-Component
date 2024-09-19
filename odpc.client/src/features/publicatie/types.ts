export const PUBLICATIESTATUS = {
  draft: "Concept",
  published: "Gepubliceerd",
  retracted: "Ingetrokken"
} as const;

export type MimeType = {
  identifier: string;
  mimeType: string;
  name: string;
};

export type PublicatieDocument = {
  uuid?: string;
  publicatie?: string;
  officieleTitel: string;
  verkorteTitel?: string;
  omschrijving?: string;
  creatiedatum?: string;

  bestandsnaam?: string;
  bestandsformaat?: string;
  bestandsomvang?: number;
};

export type Publicatie = {
  uuid?: string;
  officieleTitel: string;
  verkorteTitel: string;
  omschrijving: string;
  creatiedatum: string;
  status?: keyof typeof PUBLICATIESTATUS;
  documenten: PublicatieDocument[];
};

// export type Publicatie = {
//   uuid: string;
//   name: string;
//   status: keyof typeof PUBLICATIESTATUS;
//   documenten: Doc[];
//   createdOn: Date;
//   creator: {
//     displayName: string;
//     identifier: string;
//   }
// };
