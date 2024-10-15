import { ref, watch, computed, onMounted, type Ref } from "vue";
import { useRouter } from "vue-router";
import { useUrlSearchParams, type UrlParams } from "@vueuse/core";
import { useFetchApi } from "@/api/use-fetch-api";

const API_URL = `/api/v1`;
const PAGE_SIZE = 5;

export type PagedResult<T> = {
  count: number;
  next?: string;
  previous?: string;
  results: T[];
};

export const usePagedSearch = <T, U extends string>(endpoint: string, params: UrlParams) => {
  type QueryParams = Record<U, string> & { page: string; search: string };
  type QueryParam = keyof QueryParams;

  const requiredParams = { page: "", search: "" };

  params = { ...requiredParams, ...params };

  const paramKeys = Object.keys(params) as QueryParam[];

  const urlSearchParams = useUrlSearchParams("history", {
    initialValue: params,
    removeFalsyValues: true
  });

  const pagedResult = ref(null) as Ref<PagedResult<T> | null>;

  const queryParams = ref(params) as Ref<QueryParams>;
  const searchString = ref("");

  const router = useRouter();

  const initQueryParams = () => {
    queryParams.value = paramKeys.reduce(
      (result, key) => ({ ...result, [key]: urlSearchParams[key] || "" }),
      {} as QueryParams
    );
  };

  const onNext = () => {
    queryParams.value.page = `${+queryParams.value.page + 1}`;
  };

  const onPrev = () => {
    queryParams.value.page = `${+queryParams.value.page - 1}`;
  };

  // searchString: update queryParams.value.search when searchString.value is 'submitted from input'
  const onSearch = () => (queryParams.value = { ...queryParams.value, search: searchString.value });

  // searchString: update searchString.value when its updated from url param through queryParams.value.search
  watch(
    () => queryParams.value.search,
    (search) => (searchString.value = search)
  );

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

  const pageCount = computed(() =>
    pagedResult.value?.count ? Math.ceil(pagedResult.value.count / PAGE_SIZE) : 0
  );

  watch(searchParams, async (newValue, oldValue) => {
    const newParams = Object.fromEntries(newValue.entries());
    const oldParams = Object.fromEntries(oldValue.entries());

    // Reset page when a filter or sorting changes
    if (newParams.page === oldParams.page) {
      for (const key of paramKeys) {
        if (key === "page") continue;

        if (
          newParams.page !== "1" &&
          (newParams[key] !== oldParams[key] || newParams[key] !== urlSearchParams[key])
        ) {
          queryParams.value.page = "1";

          return false;
        }
      }
    }

    // Update history
    router.push({ query: { ...newParams } });

    // Search but wait if fetching
    await new Promise<void>((resolve) => {
      const interval = setInterval(() => {
        if (!isFetching.value) {
          clearInterval(interval);
          resolve();
        }
      }, 100);
    });

    await get().execute();
  });

  const { get, data, isFetching, error } = useFetchApi(
    () => `${API_URL}/${endpoint}/${searchParams.value.size ? "?" + searchParams.value : ""}`,
    {
      immediate: false
    }
  ).json<PagedResult<T>>();

  watch(data, (value) => (pagedResult.value = value || pagedResult.value));

  onMounted(initQueryParams);

  return {
    pagedResult,
    pageCount,
    searchString,
    queryParams,
    onNext,
    onPrev,
    onSearch,
    isFetching,
    error
  };
};
