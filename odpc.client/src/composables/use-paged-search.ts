import { ref, watch, computed, onMounted, type Ref } from "vue";
import { useRouter } from "vue-router";
import { useUrlSearchParams } from "@vueuse/core";
import { useFetchApi } from "@/api/use-fetch-api";

const API_URL = `/api/v1`;
const PAGE_SIZE = 5;

type PagedResult<T> = {
  count: number;
  next?: string;
  previous?: string;
  results: T[];
};

export const usePagedSearch = <T, U extends string>(
  endpoint: string,
  params: { page: string } & { [key in U]: string }
) => {
  type QueryParams = typeof params;

  const router = useRouter();

  const urlSearchParams = useUrlSearchParams("history", {
    initialValue: params,
    removeFalsyValues: true
  });

  const pagedResult = ref(null) as Ref<PagedResult<T> | null>;

  const queryParams = ref(params) as Ref<QueryParams>;
  const paramKeys = Object.keys(params) as (keyof QueryParams)[];

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

  const pageCount = computed(() =>
    pagedResult.value?.count ? Math.ceil(pagedResult.value.count / PAGE_SIZE) : 0
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

  watch(searchParams, async (newParams, oldParams) => {
    // Reset to page 1 when a filter or the sorting changes
    if (newParams.get("page") === oldParams.get("page") && queryParams.value.page !== "1") {
      queryParams.value.page = "1";

      return false;
    }

    // Update history
    router.replace({ query: { ...Object.fromEntries(newParams.entries()) } });

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
    queryParams,
    pageCount,
    onNext,
    onPrev,
    isFetching,
    error
  };
};
