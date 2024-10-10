import { ref, watch, computed, onMounted, type Ref } from "vue";
import { useUrlSearchParams, type UrlParams } from "@vueuse/core";
import { useFetchApi } from "@/api/use-fetch-api";

const API_URL = `/api/v1`;

export type RequiredPaginatedSearchParams = { page: string; search: string };

export const usePaginatedSearch = <T, U extends string>(endpoint: string, params: UrlParams) => {
  type QueryParams = Record<U, string> & RequiredPaginatedSearchParams;
  type QueryParam = keyof QueryParams;

  const requiredParams: RequiredPaginatedSearchParams = { page: "", search: "" };

  params = { ...requiredParams, ...params };

  const paramKeys = Object.keys(params) as QueryParam[];

  const urlSearchParams = useUrlSearchParams("history", {
    initialValue: params,
    removeFalsyValues: true
  });

  const items = ref<T>();

  const queryParams = ref(params) as Ref<QueryParams>;
  const searchString = ref("");

  const initQueryParams = () => {
    queryParams.value = paramKeys.reduce(
      (result, key) => ({ ...result, [key]: urlSearchParams[key] || "" }),
      {} as QueryParams
    );
  };

  const nextPage = () => {
    queryParams.value.page = `${+queryParams.value.page + 1}`;
  };

  const prevPage = () => {
    queryParams.value.page = `${+queryParams.value.page - 1}`;
  };

  const onSearch = () => (queryParams.value = { ...queryParams.value, search: searchString.value });

  const searchParams = computed(
    () =>
      new URLSearchParams(
        paramKeys.reduce(
          (result, key) =>
            queryParams.value[key] ? { ...result, [key]: queryParams.value[key] } : result,
          {}
        )
      )
  );

  watch(
    () => queryParams.value.search,
    (search) => (searchString.value = search)
  );

  watch(searchParams, async (value) => {
    // Clear all urlSearchParams
    for (const k in urlSearchParams) urlSearchParams[k] = "";

    // Set new urlSearchParams
    for (const [k, v] of value) urlSearchParams[k] = v;

    await get().execute();
  });

  const { get, data, isFetching, error } = useFetchApi(
    () => `${API_URL}/${endpoint}${searchParams.value.size ? "?" + searchParams.value : ""}`,
    {
      immediate: false
    }
  ).json<T>();

  watch(data, (value) => (items.value = value || items.value));

  onMounted(initQueryParams);

  return {
    items,
    searchString,
    queryParams,
    nextPage,
    prevPage,
    onSearch,
    isFetching,
    error
  };
};
