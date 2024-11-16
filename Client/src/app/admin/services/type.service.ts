import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient,HttpHeaders ,HttpParams } from '@angular/common/http';
import { ITypePagination } from 'src/app/shared/models/typePagination';
import { map } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class TypeService {

  baseUrl: string = environment.apiUrl;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private http: HttpClient) { }

  getTypesPagination(paginationData:any) {
    let params = new HttpParams();

    params = params.append("pageIndex", paginationData.pageIndex);
    params = params.append("pageSize", paginationData.pageSize);


    if (paginationData.search) {
      params = params.append("search", paginationData.search);
    }

    return this.http.get<ITypePagination>(this.baseUrl + "ProductTypes/", {
      observe: "response",
      params: params

    }).pipe(
      map(response => {
        return response.body;
      })
    )

  }

  postType(data:any){
    console.log(data);
    return this.http.post<any>(this.baseUrl+ "ProductTypes",data);

  }

  deleteType(id:number){
    return this.http.delete<any>(this.baseUrl+ "ProductTypes/"+id);

  }

  updateType(id:number,data:any){
    return this.http.put<any>(this.baseUrl+ "ProductTypes/"+id,data);
  }

}
