import { ref } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import type { Bestandsdeel, MimeType } from "./types";

const API_URL = `/api/v1`;

const mimeTypesMap = ref<Map<string, MimeType> | null>(null);

(async () => {
  const { data } = await useFetchApi(() => `/api/v1/formats`).json<MimeType[]>();

  mimeTypesMap.value = new Map(data.value?.map((type) => [type.mimeType, type]));
})();

const parseUrl = (url: string) => {
  const regex = /\/documenten\/(?<docUuid>[a-f0-9-]+)\/bestandsdelen\/(?<partUuid>[a-f0-9-]+)/;
  const match = url.match(regex);

  if (match && match.groups) {
    return match.groups;
  } else {
    throw new Error("Invalid URL format");
  }
};

const uploadFile = async (file: File, bestandsdelen: Bestandsdeel[]) => {
  let blobStart = 0;

  try {
    for (const { url, omvang } of bestandsdelen) {
      const { docUuid, partUuid } = parseUrl(url);
      const body = new FormData();
      const blob = file.slice(blobStart, blobStart + omvang);

      body.append("inhoud", blob);

      const { status } = await fetch(`${API_URL}/documenten/${docUuid}/bestandsdelen/${partUuid}`, {
        method: "PUT",
        body
      });

      if (status !== 204) {
        throw new Error(`Error uploadDocument: ${url}`);
      }

      blobStart += omvang;
    }
  } catch {
    throw new Error();
  }
};

export { mimeTypesMap, uploadFile };
