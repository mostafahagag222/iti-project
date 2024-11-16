import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { HttpClient,HttpHeaders ,HttpParams } from '@angular/common/http';
import { IBrandPagination } from 'src/app/shared/models/brandPagination';
import { IBrand } from 'src/app/shared/models/brand';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BrandService {
  baseUrl: string = environment.apiUrl;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private http: HttpClient) { }


  getBrandsPagination(paginationData:any) {
    let params = new HttpParams();

    params = params.append("pageIndex", paginationData.pageIndex);
    params = params.append("pageSize", paginationData.pageSize);


    if (paginationData.search) {
      params = params.append("search", paginationData.search);
    }

    return this.http.get<IBrandPagination>(this.baseUrl + "ProductBrands/", {
      observe: "response",
      params: params

    }).pipe(
      map(response => {
        return response.body;
      })
    )

  }

  postBrand(data:any){
    console.log(data);
    return this.http.post<any>(this.baseUrl+ "ProductBrands",data);

  }

  deleteBrand(id:number){
    return this.http.delete<any>(this.baseUrl+ "ProductBrands/"+id);

  }

  updateBrand(id:number,data:any){
    return this.http.put<any>(this.baseUrl+ "ProductBrands/"+id,data);
  }


}
