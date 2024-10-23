import { useFetchApi } from "@/api";
import type { WaardelijstItem } from "../types";

const API_URL = `/api/v1`;

export const useWaardelijstenUser = () => {
  const {
    data: mijnInformatiecategorieen,
    isFetching: loadingWaardelijstenUser,
    error: waardelijstenUserError
  } = useFetchApi(() => `${API_URL}/mijn-informatiecategorieen`).json<WaardelijstItem[]>();

  return {
    mijnInformatiecategorieen,
    loadingWaardelijstenUser,
    waardelijstenUserError
  };
};
