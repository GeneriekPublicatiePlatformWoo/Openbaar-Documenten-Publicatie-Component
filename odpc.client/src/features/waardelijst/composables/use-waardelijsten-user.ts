import { computed } from "vue";
import { useFetchApi, type PagedResult } from "@/api";
import type { WaardelijstItem } from "../types";

const API_URL = `/api/v1`;

export const useWaardelijstenUser = () => {
  const {
    data,
    isFetching: loadingWaardelijstenUser,
    error: waardelijstenUserError
  } = useFetchApi(() => `${API_URL}/mijn-informatiecategorieen`).json<
    PagedResult<WaardelijstItem>
  >();

  const mijnInformatiecategorieen = computed(() => data.value?.results || []);

  return {
    mijnInformatiecategorieen,
    loadingWaardelijstenUser,
    waardelijstenUserError
  };
};
