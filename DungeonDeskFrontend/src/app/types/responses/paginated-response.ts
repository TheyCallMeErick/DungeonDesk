import PaginationResponse from './pagination-response';

export default class PaginatedResponse<T> {
  items: T[] = [];
  pagination: PaginationResponse = new PaginationResponse();
}
