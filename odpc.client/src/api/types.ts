export type PagedResult<T> = {
  count: number;
  next?: string;
  previous?: string;
  results: T[];
};
