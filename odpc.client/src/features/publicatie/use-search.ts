import { ref, watch, computed, onMounted, type Ref } from "vue";
import { useUrlSearchParams, type UrlParams } from "@vueuse/core";
import { useFetchApi } from "@/api/use-fetch-api";

const API_URL = `/api/v1`;

export const useSearch = <T, U extends string>(endpoint: string, params: UrlParams) => {
  type QueryParams = Record<U, string> & { query: string };

  const urlSearchParams = useUrlSearchParams("history", {
    initialValue: params,
    removeFalsyValues: true
  });

  const items = ref<T>();

  const queryParams = ref(params) as Ref<QueryParams>;
  const queryString = ref("");

  const onInit = () => {
    queryParams.value = (Object.keys(params) as (keyof QueryParams)[]).reduce((result, key) => {
      (result as Record<string, string>)[key] = (urlSearchParams[key] as string) || "";

      return result;
    }, {} as QueryParams);
  };

  const onSearch = () => (queryParams.value = { ...queryParams.value, query: queryString.value });

  const searchParams = computed(
    () =>
      new URLSearchParams(
        (Object.keys(params) as (keyof QueryParams)[]).reduce((result, key) => {
          if (queryParams.value[key]) result[key] = queryParams.value[key];

          return result;
        }, {} as QueryParams)
      )
  );

  watch(
    () => queryParams.value.query,
    (query) => {
      queryString.value = query;
    }
  );

  watch(searchParams, async (value) => {
    // Clear all urlSearchParams
    for (const k in urlSearchParams) urlSearchParams[k as keyof QueryParams] = "";

    // Set new urlSearchParams
    for (const [k, v] of value) urlSearchParams[k as keyof QueryParams] = v;

    await get().execute();
  });

  const { get, data, isFetching, error } = useFetchApi(
    () => `${API_URL}/${endpoint}${searchParams.value.size ? "?" + searchParams.value : ""}`,
    {
      immediate: false
    }
  ).json<T>();

  watch(data, (value) => (items.value = value || items.value));

  onMounted(async () => onInit());

  return {
    items,
    queryString,
    queryParams,
    onSearch,
    isFetching,
    error
  };
};
