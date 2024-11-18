import { ref } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import type { Bestandsdeel, MimeType } from "./types";

const API_URL = `/api/v1`;

const mimeTypesMap = ref<Map<string, MimeType> | null>(null);

(async () => {
  const { data } = await useFetchApi(() => `${API_URL}/formats`).json<MimeType[]>();

  mimeTypesMap.value = new Map(data.value?.map((type) => [type.mimeType, type]));
})();

const uploadFile = async (file: File, bestandsdelen: Bestandsdeel[]) => {
  let blobStart = 0;

  bestandsdelen.sort((a, b) => a.volgnummer - b.volgnummer);

  try {
    for (const { url, omvang } of bestandsdelen) {
      const body = new FormData();
      const blob = file.slice(blobStart, blobStart + omvang);

      body.append("url", url);
      body.append("inhoud", blob);
      
      const { ok } = await fetch(`${API_URL}/bestandsdelen`, {
        method: "PUT",
        body
      });

      if (!ok) {
        throw new Error(`Error uploadDocument: ${url}`);
      }

      blobStart += omvang;
    }
  } catch {
    throw new Error();
  }
};

export { mimeTypesMap, uploadFile };
