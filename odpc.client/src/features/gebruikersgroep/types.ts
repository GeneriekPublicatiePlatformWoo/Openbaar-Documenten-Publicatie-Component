export type Gebruikersgroep = {
  uuid?: string;
  naam: string;
  omschrijving?: string;
  gekoppeldeWaardelijsten: string[];
  gekoppeldeGebruikers: string[];
};
