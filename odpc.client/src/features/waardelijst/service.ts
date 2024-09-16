import { computed } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import {
  WAARDELIJSTEN,
  type GroupedWaardeLijstItems,
  type Waardelijst,
  type WaardelijstItem
} from "./types";

export const getWaardelijsten = () => {
  const {
    data,
    isFetching: loadingListItems,
    error: listItemstError
  } = useFetchApi(`/waardelijsten`).json<WaardelijstItem[]>();

  const groupedItems = computed<GroupedWaardeLijstItems>(() =>
    Object.keys(WAARDELIJSTEN).reduce((result: GroupedWaardeLijstItems, key) => {
      result[key as Waardelijst] =
        data.value
          ?.filter((item) => item.type === key)
          ?.sort((a, b) => a.name.localeCompare(b.name)) || [];
      return result;
    }, {} as GroupedWaardeLijstItems)
  );

  return { groupedItems, loadingListItems, listItemstError };
};
