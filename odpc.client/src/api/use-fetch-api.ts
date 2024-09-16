import { createFetch } from "@vueuse/core";

export const useFetchApi = createFetch({
  baseUrl: "/api",
  options: {
    beforeFetch({ options }) {
      // const token = "token";

      options.headers = {
        ...options.headers,
        "content-type": "application/json"
        // Authorization: `Bearer ${token}`
      };

      return { options };
    },
    afterFetch(ctx) {
      // console.log(ctx);
      return ctx;
    },
    onFetchError(ctx) {
      // console.log(ctx);
      return ctx;
    }
  }
});
