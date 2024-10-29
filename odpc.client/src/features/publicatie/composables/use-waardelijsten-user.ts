import { useFetchApi } from "@/api";
import type { OptionProps } from "@/components/checkbox-group/types";

const API_URL = `/api/v1`;

export const useWaardelijstenUser = () => {
  const {
    data: mijnInformatiecategorieen,
    isFetching: loadingWaardelijstenUser,
    error: waardelijstenUserError
  } = useFetchApi(() => `${API_URL}/mijn-informatiecategorieen`).json<OptionProps[]>();

  return {
    mijnInformatiecategorieen,
    loadingWaardelijstenUser,
    waardelijstenUserError
  };
};
