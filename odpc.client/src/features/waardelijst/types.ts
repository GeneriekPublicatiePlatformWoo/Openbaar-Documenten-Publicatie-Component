import type { OptionProps } from "@/components/CheckboxList.vue";

export const WAARDELIJSTEN = {
  ORGANISATIE: "Organisatie",
  INFORMATIECATEGORIE: "Informatiecategorie",
  THEMA: "Thema"
  // DOCUMENTSOORT: 'Documentsoort'
} as const;

export type Waardelijst = keyof typeof WAARDELIJSTEN;

export type WaardelijstItem = {
  type: Waardelijst;
} & OptionProps;

export type GroupedWaardeLijstItems = {
  [key in Waardelijst]: WaardelijstItem[];
};
