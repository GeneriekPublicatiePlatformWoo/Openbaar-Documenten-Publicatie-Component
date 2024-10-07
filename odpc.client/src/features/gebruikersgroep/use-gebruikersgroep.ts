import { ref, onMounted, watch } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import toast from "@/stores/toast";
import type { Gebruikersgroep } from "./types";
import { waardelijstIds } from "@/features/waardelijst";

const GGAPI_URL = `/api/v1/gebruikersgroepen`;

export const useGebruikersgroep = (uuid?: string) => {
  const gebruikersgroep = ref<Gebruikersgroep>({
    naam: "",
    omschrijving: "",
    gekoppeldeWaardelijsten: [],
    gekoppeldeGebruikers: []
  });

  const {
    get: getGebruikersgroep,
    post: postGebruikersgroep,
    put: putGebruikersgroep,
    delete: deleteGebruikersgroep,
    data: gebruikersgroepData,
    isFetching: loadingGebruikersgroep,
    error: gebruikersgroepError
  } = useFetchApi(() => `${GGAPI_URL}${uuid ? "/" + uuid : ""}`, {
    immediate: false
  }).json<Gebruikersgroep>();

  watch(gebruikersgroepData, (value) => (gebruikersgroep.value = value || gebruikersgroep.value));

  const submitGebruikersgroep = async () => {
    gebruikersgroep.value = {
      ...gebruikersgroep.value,
      gekoppeldeWaardelijsten: gebruikersgroep.value.gekoppeldeWaardelijsten.filter((id) =>
        waardelijstIds.value.includes(id)
      )
    };

    uuid
      ? await putGebruikersgroep(gebruikersgroep).execute()
      : await postGebruikersgroep(gebruikersgroep).execute();

    if (gebruikersgroepError.value) {
      toast.add({
        text: "De gebruikersgroep kon niet worden opgeslagen, probeer het nogmaals...",
        type: "error"
      });

      gebruikersgroepError.value = null;

      throw new Error();
    }
  };

  const removeGebruikersgroep = async () => {
    await deleteGebruikersgroep().text().execute();

    if (gebruikersgroepError.value) {
      toast.add({
        text: "De gebruikersgroep kon niet worden verwijderd, probeer het nogmaals...",
        type: "error"
      });

      gebruikersgroepError.value = null;

      throw new Error();
    }
  };

  onMounted(async () => uuid && (await getGebruikersgroep().execute()));

  return {
    gebruikersgroep,
    loadingGebruikersgroep,
    gebruikersgroepError,
    submitGebruikersgroep,
    removeGebruikersgroep
  };
};
