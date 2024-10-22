export const WAARDELIJSTEN = {
  ORGANISATIE: "Organisatie",
  INFORMATIECATEGORIE: "Informatiecategorie"
  // THEMA: "Thema"
  // DOCUMENTSOORT: 'Documentsoort'
} as const;

export type OptionProps = {
  uuid: string;
  naam: string;
};

export type WaardelijstItem = OptionProps &
  Partial<{
    identifier: string;
    naamMeervoud: string;
    definitie: string;
    oorsprong: string;
    order: number;
  }>;
