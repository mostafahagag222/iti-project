import { DecimalPipe } from "@angular/common";
import { InterpolationConfig } from "@angular/compiler";
import { IAddress } from "./address";

export interface IOrderToCreate {
  basketId : number;
  deliveryMethodId : number;
  shipToAddress : IAddress;
}

export interface IOrder{
  id : number;
  buyerEmail : string;
  orderDate : string;
  shipToAddress : IAddress;
  deliveryMethod : string;
  shippingPrice : number;
  orderItems : IOrderItem[];
  subtotal : number;
  total : number;
  status : string;
}

export interface IOrderItem{
  productId : number;
  productName : string;
  price : number;
  quantity : number;
}


export class orderParams {
  pageIndex: number;
  pageSize: number;
  subTotalFrom : number;
  subTotalTo : number;
  sort : string;
  search: string;
}
