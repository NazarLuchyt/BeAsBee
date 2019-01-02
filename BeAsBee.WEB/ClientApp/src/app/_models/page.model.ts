export interface Page<T> {
    items: T[];
    count: number;
    pageNumber: number;
}