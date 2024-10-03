import { ref, onMounted, watch } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import toast from "@/stores/toast";
import type { Publicatie } from "./types";

const PUBAPI_URL = `/api/v1/publicaties`;

export const usePublicatie = (uuid?: string) => {
  const publicatie = ref<Publicatie>({
    officieleTitel: "",
    verkorteTitel: "",
    omschrijving: "",
    creatiedatum: new Date().toISOString().split("T")[0]
  });

  const {
    get: getPublicatie,
    post: postPublicatie,
    put: putPublicatie,
    data: publicatieData,
    isFetching: loadingPublicatie,
    error: publicatieError
  } = useFetchApi(() => `${PUBAPI_URL}${uuid ? "/" + uuid : ""}`, {
    immediate: false
  }).json<Publicatie>();

  watch(publicatieData, (value) => (publicatie.value = value || publicatie.value));

  const submitPublicatie = async () => {
    uuid ? await putPublicatie(publicatie).execute() : await postPublicatie(publicatie).execute();

    if (publicatieError.value) {
      toast.add({
        text: "De publicatie kon niet worden opgeslagen, probeer het nogmaals...",
        type: "error"
      });

      publicatieError.value = null;

      throw new Error();
    }
  };

  onMounted(async () => uuid && (await getPublicatie().execute()));

  return {
    publicatie,
    loadingPublicatie,
    publicatieError,
    submitPublicatie
  };
};
