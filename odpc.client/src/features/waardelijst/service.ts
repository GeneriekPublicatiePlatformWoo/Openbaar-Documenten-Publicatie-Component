import { computed } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import {
  WAARDELIJSTEN,
  type GroupedWaardelijstItems,
  type Waardelijst,
  type WaardelijstItem
} from "./types";

export const getWaardelijsten = () => {
  const {
    data,
    isFetching: loadingListItems,
    error: listItemstError
  } = useFetchApi(`/waardelijsten`).json<WaardelijstItem[]>();

  const groupedWaardelijstItems = computed<GroupedWaardelijstItems>(() =>
    Object.keys(WAARDELIJSTEN).reduce((result: GroupedWaardelijstItems, key) => {
      result[key as Waardelijst] =
        data.value
          ?.filter((item) => item.type === key)
          ?.sort((a, b) => a.name.localeCompare(b.name)) || [];
      return result;
    }, {} as GroupedWaardelijstItems)
  );

  const waardelijstIds = computed(() => data.value?.map((item) => item.id) || []);

  return { groupedWaardelijstItems, waardelijstIds, loadingListItems, listItemstError };
};
