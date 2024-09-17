export const PUBLICATIESTATUS = {
  draft: "Concept",
  published: "Gepubliceerd",
  retracted: "Ingetrokken"
} as const;

type OrganisatieReference = {
  name: string;
  identifier: string;
};

export type Doc = {
  uitgever: OrganisatieReference;
  verantwoordelijke: OrganisatieReference;
  titel: string;
  informatiecategorie: {
    name: string;
    identifier: string;
    origin: string;
  }
};

export type Publicatie = {
  identifier?: string;
  officieleTitel: string;
  verkorteTitel: string;
  omschrijving: string;
  creatiedatum: string;
  status?: keyof typeof PUBLICATIESTATUS;
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
