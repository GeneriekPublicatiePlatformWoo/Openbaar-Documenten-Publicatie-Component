import type { OptionProps } from "@/components/checkbox-group/types";

export const WAARDELIJSTEN = {
  ORGANISATIE: "Organisatie",
  INFORMATIECATEGORIE: "Informatiecategorie"
  // THEMA: "Thema"
  // DOCUMENTSOORT: 'Documentsoort'
} as const;

export type WaardelijstItem = OptionProps &
  Partial<{
    identifier: string;
    naamMeervoud: string;
    definitie: string;
    oorsprong: string;
    order: number;
  }>;
