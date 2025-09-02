export default class DeskQueryRequest {
  name: string = '';
  description: string = '';
  table_status: string = '';
  max_players: number = 0;
  is_full: boolean = false;
  page: number = 1;
  page_size: number = 10;
}
