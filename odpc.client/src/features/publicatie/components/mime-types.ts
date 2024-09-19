import { ref } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import type { MimeTypes } from "../types";

const mimeTypesMap = ref<Map<string, MimeTypes> | null>(null);

const fetchFileTypes = async () => {
  const { data } = await useFetchApi(() => `/api-mock/v1/formats`).json<MimeTypes[]>();

  mimeTypesMap.value = new Map(data.value?.map((type) => [type.mimeType, type]));
};

fetchFileTypes();

export { mimeTypesMap };
