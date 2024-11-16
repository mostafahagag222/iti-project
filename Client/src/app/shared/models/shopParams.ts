export class ShopParams {
  brandId: number = 0;
  typeId: number = 0;
  sort = "name";
  pageIndex = 1;
  pageSize = 6;
  search: string;
  priceFrom: number = 0;
  priceTo: number = 100000;
  color = "";
}
