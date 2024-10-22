import { useFetchApi } from "@/api";
import type { WaardelijstItem } from "../types";

export const useWaardelijstenUser = () => {
  const {
    data: mijnInformatiecategorieen,
    isFetching: loadingWaardelijstenUser,
    error: waardelijstenUserError
  } = useFetchApi(() => `${import.meta.env.VITE_API_URL}/mijn-informatiecategorieen`).json<
    WaardelijstItem[]
  >();

  return {
    mijnInformatiecategorieen,
    loadingWaardelijstenUser,
    waardelijstenUserError
  };
};
