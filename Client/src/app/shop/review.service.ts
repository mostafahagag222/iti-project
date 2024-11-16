import { IProductReviews } from './../shared/models/productReviews';
import { IReview } from './../shared/models/review';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  baseUrl: string = environment.apiUrl;
  private currentReviews = new BehaviorSubject<IProductReviews>(null);
  Reviews$ = this.currentReviews.asObservable();

  constructor(private http: HttpClient) { }


  getReviews(productId: number) {

    return this.http.get<IProductReviews>(this.baseUrl + "review/" + productId).pipe(
      map((response: IProductReviews) => {
        this.currentReviews.next(response)
        // console.log(response)
        return response;
      })
    )
  }

  addReview(token: string, productId: number, body: string, stars: number) {

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`)

    let bodyRequest = {
      productId,
      body,
      stars
    }

    return this.http.post(this.baseUrl + 'review', bodyRequest, { headers }).pipe(
      map((response: IProductReviews) => {
        this.currentReviews.next(response)
        // console.log(response)
      })
    )
  }
}
