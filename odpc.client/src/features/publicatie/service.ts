import { ref } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import type { Bestandsdeel, MimeTypes } from "./types";

const mimeTypesMap = ref<Map<string, MimeTypes> | null>(null);

(async () => {
  const { data } = await useFetchApi(() => `/api/v1/formats`).json<MimeTypes[]>();

  mimeTypesMap.value = new Map(data.value?.map((type) => [type.mimeType, type]));
})();

const uploadFile = async (file: File, bestandsdelen: Bestandsdeel[]): Promise<void> => {
  let blobStart = 0;

  try {
    for (const { url, omvang } of bestandsdelen) {
      const body = new FormData();
      const blob = file.slice(blobStart, blobStart + omvang);

      body.append("inhoud", blob);

      const { error } = await useFetchApi(url).put(body);

      if (error.value) {
        throw new Error(`Error uploadDocument: ${url}`);
      }

      blobStart += omvang;
    }
  } catch {
    throw new Error();
  }
};

export { mimeTypesMap, uploadFile };
