import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { HttpClient,HttpHeaders ,HttpParams } from '@angular/common/http';

import { IBrand } from 'src/app/shared/models/brand';
import { IType } from 'src/app/shared/models/productType';
import { IPagination } from 'src/app/shared/models/pagination';
import { ShopParams } from 'src/app/shared/models/shopParams';
import { IBrandPagination } from 'src/app/shared/models/brandPagination';

import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseUrl: string = environment.apiUrl;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }
  constructor(private http: HttpClient) { }

  getProducts(shopParams: ShopParams) {

    let params = new HttpParams();


    params = params.append("pageIndex", shopParams.pageIndex);
    params = params.append("pageSize", shopParams.pageSize);


    if (shopParams.search) {
      params = params.append("search", shopParams.search);
    }


    return this.http.get<IPagination>(this.baseUrl + "products", {
      observe: "response",
      params: params

    }).pipe(
      map(response => {
        return response.body;
      })
    )


    // return this.http.get<any>(this.baseUrl+ "products");
  }


  postProduct(data:any){
    console.log(data);
    return this.http.post<any>(this.baseUrl+ "products",data);

  }

  deleteProduct(id:number){
    return this.http.delete<any>(this.baseUrl+ "products/"+id);

  }


  updateProduct(id:number,data:any){
    return this.http.put<any>(this.baseUrl+ "products/"+id,data);
  }



  getBrands() {
    return this.http.get<IBrand[]>(this.baseUrl + "products/brands");
  }

  getTypes() {
    return this.http.get<IType[]>(this.baseUrl + "products/types");
  }




}
