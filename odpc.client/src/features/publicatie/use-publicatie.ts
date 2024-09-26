import { ref, onMounted, watch } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import toast from "@/stores/toast";
import type { Publicatie } from "./types";

export const usePublicatie = (uuid: string | undefined) => {
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
  } = useFetchApi(() => `/api/v1/publicaties${uuid ? "/" + uuid : ""}`, {
    immediate: false
  }).json<Publicatie>();

  watch(publicatieData, (value) => (publicatie.value = value || publicatie.value), {
    immediate: false
  });

  const submitPublicatie = async (): Promise<void> => {
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