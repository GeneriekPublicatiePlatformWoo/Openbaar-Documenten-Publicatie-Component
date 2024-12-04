import { ref, type Ref } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import type { Bestandsdeel, MimeType } from "./types";

const mimeTypesMap = ref<Map<string, MimeType> | null>(null);

(async () => {
  const { data } = await useFetchApi(() => "/api/v1/formats").json<MimeType[]>();

  mimeTypesMap.value = new Map(data.value?.map((type) => [type.mimeType, type]));
})();

const uploadFile = async (file: File, bestandsdelen: Bestandsdeel[], progress?: Ref<number>) => {
  let blobStart = 0;

  bestandsdelen.sort((a, b) => a.volgnummer - b.volgnummer);

  try {
    for (const { url, omvang } of bestandsdelen) {
      const { pathname, search } = new URL(url);

      const body = new FormData();
      const blob = file.slice(blobStart, blobStart + omvang);

      body.append("inhoud", blob);

      const ok = await uploadWithProgress(pathname + search, "PUT", body, progress);

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

async function uploadWithProgress(
  url: string,
  method: string,
  blob: Document | XMLHttpRequestBodyInit,
  progress?: Ref<number>
) {
  const xhr = new XMLHttpRequest();
  return new Promise<boolean>((resolve) => {
    progress &&
      xhr.upload.addEventListener("progress", (event) => {
        if (event.lengthComputable) {
          progress.value = Math.round((event.loaded / event.total) * 100);
        }
      });
    progress &&
      xhr.addEventListener("progress", (event) => {
        if (event.lengthComputable) {
          progress.value = Math.round((event.loaded / event.total) * 100);
        }
      });
    xhr.addEventListener("loadend", () => {
      resolve(xhr.readyState === 4 && xhr.status < 400);
    });
    xhr.open(method, url, true);
    xhr.setRequestHeader("is-api", "true");
    xhr.send(blob);
  });
}
