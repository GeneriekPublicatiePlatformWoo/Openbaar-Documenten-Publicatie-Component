import { computed, ref } from "vue";
import { useFetchApi, type PagedResult } from "@/api";
import type { WaardelijstItem } from "./types";

const API_URL = `/api/v1`;

const organisaties = ref<WaardelijstItem[]>();
const informatiecategorieen = ref<WaardelijstItem[]>();

const loadingWaardelijsten = ref(false);
const waardelijstenError = ref<string>();

(async () => {
  loadingWaardelijsten.value = true;

  try {
    const [{ data: organisatieData }, { data: informatiecategorieData }] = await Promise.all([
      useFetchApi(`${API_URL}/organisaties`).json<PagedResult<WaardelijstItem>>(),
      useFetchApi(`${API_URL}/informatiecategorieen`).json<PagedResult<WaardelijstItem>>()
    ]);

    organisaties.value = organisatieData.value?.results;
    informatiecategorieen.value = informatiecategorieData.value?.results;
  } catch (err) {
    waardelijstenError.value = "Error";
  } finally {
    loadingWaardelijsten.value = false;
  }
})();

const waardelijstIds = computed(() =>
  [...(organisaties.value || []), ...(informatiecategorieen.value || [])].map((item) => item.uuid)
);

export {
  organisaties,
  informatiecategorieen,
  waardelijstIds,
  loadingWaardelijsten,
  waardelijstenError
};
