import { ref, watch, computed, onMounted, type Ref } from "vue";
import { useUrlSearchParams, type UrlParams } from "@vueuse/core";
import { useFetchApi } from "@/api/use-fetch-api";

const API_URL = `/api/v1`;

export const usePaginatedSearch = <T, U extends string>(endpoint: string, params: UrlParams) => {
  type RequiredParams = { page: string; search: string };
  type QueryParams = Record<U, string> & RequiredParams;
  type QueryParam = keyof QueryParams;

  const requiredParams = { page: "", search: "" };

  params = { ...requiredParams, ...params };

  const urlSearchParams = useUrlSearchParams("history", {
    initialValue: params,
    removeFalsyValues: true
  });

  const items = ref<T>();

  const queryParams = ref(params) as Ref<QueryParams>;
  const searchString = ref("");

  const initQueryParams = () => {
    queryParams.value = (Object.keys(params) as QueryParam[]).reduce((result, key) => {
      (result as Record<string, string>)[key] = (urlSearchParams[key] as string) || "";

      return result;
    }, {} as QueryParams);
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
        (Object.keys(params) as QueryParam[]).reduce((result, key) => {
          if (queryParams.value[key]) result[key] = queryParams.value[key];

          return result;
        }, {} as QueryParams)
      )
  );

  watch(
    () => queryParams.value.search,
    (search) => (searchString.value = search)
  );

  watch(searchParams, async (value) => {
    // Clear all urlSearchParams
    for (const k in urlSearchParams) urlSearchParams[k as QueryParam] = "";

    // Set new urlSearchParams
    for (const [k, v] of value) urlSearchParams[k as QueryParam] = v;

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
