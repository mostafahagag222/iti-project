import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NumberValueAccessor } from '@angular/forms';
import { map } from 'rxjs';
import { IOrder, orderParams } from 'src/app/shared/models/order';
import { IOrdersPagination } from 'src/app/shared/models/pagination';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminOrdersService {

  baseUrl = environment.apiUrl;

  constructor(private http : HttpClient) { }

  getAllOrders(orderParamsToSend : orderParams) {

    let params = new HttpParams();

    if (orderParamsToSend.subTotalFrom !== 0) {
      params = params.append("SubTotalFrom", orderParamsToSend.subTotalFrom.toString());
    }

    if (orderParamsToSend.subTotalTo) {
      params = params.append("SubTotalTo", orderParamsToSend.subTotalTo.toString());
    }

    if (orderParamsToSend.sort) {
      params = params.append("Sort", orderParamsToSend.sort);
    }

    if (orderParamsToSend.search) {
      params = params.append("Search", orderParamsToSend.search);
    }

    params = params.append("PageIndex", orderParamsToSend.pageIndex);
    params = params.append("PageSize", orderParamsToSend.pageSize);

    return this.http.get<IOrdersPagination>(this.baseUrl + "AdminOrders", {
      observe: "response",
      params: params
    }).pipe(
      map(response => {
        console.log(response);
        return response.body;
      })
    )
  }

  getMaxPrice() {
    return this.http.get<number>(this.baseUrl + "AdminOrders/maxPrice");
  }

  confirmAllOrders() {
    return this.http.put<null>(this.baseUrl + "AdminOrders/confirmAllOrders", {
      observe : "response",
      params : null
    });
  }

  confirmOrder(id : number, order : IOrder) {
    order.status = "Confirmed";
    return this.http.put<null>(this.baseUrl + "AdminOrders/" + id,{
      observe: "response",
      params:null
    });
  }

  deleteOrder(id : number){
    return this.http.delete<IOrder>(this.baseUrl + "Orders/" + id);
  }

  getOrder(id : number) {
    return this.http.get<IOrder>(this.baseUrl + "AdminOrders/" + id);
  }
}
