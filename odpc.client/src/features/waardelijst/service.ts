import { computed } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import {
  WAARDELIJSTEN,
  type GroupedWaardelijstItems,
  type Waardelijst,
  type WaardelijstItem
} from "./types";

const {
  data,
  isFetching: loadingWaardelijstItems,
  error: waardelijstItemsError
} = useFetchApi(`/api/v1/waardelijsten`).json<WaardelijstItem[]>();

const waardelijstIds = computed(() => data.value?.map((item) => item.id) || []);

const groupedWaardelijstItems = computed<GroupedWaardelijstItems>(() =>
  Object.keys(WAARDELIJSTEN).reduce((result: GroupedWaardelijstItems, key) => {
    result[key as Waardelijst] =
      data.value
        ?.filter((item) => item.type === key)
        ?.sort((a, b) => a.name.localeCompare(b.name)) || [];
    return result;
  }, {} as GroupedWaardelijstItems)
);

export { waardelijstIds, groupedWaardelijstItems, loadingWaardelijstItems, waardelijstItemsError };
