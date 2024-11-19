import { computed } from "vue";
import { useFetchApi } from "@/api";
import type { OptionProps } from "@/components/option-group/types";

export const useWaardelijstenUser = () => {
  const {
    data: mijnOrganisaties,
    isFetching: loadingMijnOrganisaties,
    error: mijnOrganisatiesError
  } = useFetchApi(() => `/api/v1/mijn-organisaties`).json<OptionProps[]>();

  const {
    data: mijnInformatiecategorieen,
    isFetching: loadingMijnInformatiecategorieen,
    error: mijnInformatiecategorieenError
  } = useFetchApi(() => `/api/v1/mijn-informatiecategorieen`).json<OptionProps[]>();

  const loadingWaardelijstenUser = computed(
    () => loadingMijnOrganisaties.value || loadingMijnInformatiecategorieen.value
  );
  const waardelijstenUserError = computed(
    () => mijnOrganisatiesError.value || mijnInformatiecategorieenError.value
  );

  const mijnWaardelijstenUuids = computed(() => [
    ...(mijnOrganisaties.value?.map((item) => item.uuid) || []),
    ...(mijnInformatiecategorieen.value?.map((item) => item.uuid) || [])
  ]);

  return {
    mijnOrganisaties,
    mijnInformatiecategorieen,
    mijnWaardelijstenUuids,
    loadingWaardelijstenUser,
    waardelijstenUserError
  };
};
