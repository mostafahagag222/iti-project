import { IProduct } from 'src/app/shared/models/product';
import { IWishList } from './../shared/models/wishlist';
import { environment } from './../../environments/environment';
import { IUser } from './../shared/models/user';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Router } from '@angular/router';
import { BasketService } from '../basket/basket.service';
import { IUserPagination } from 'src/app/shared/models/userPagination';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<IUser>(null);
  currentUser$ = this.currentUserSource.asObservable();
  private currentCustomerUsersSource = new BehaviorSubject<IUser[]>(null);
  currentCustomerUsers$ = this.currentCustomerUsersSource.asObservable();
  private currentUWishListSource = new BehaviorSubject<IWishList>(null);
  WishList$ = this.currentUWishListSource.asObservable();

  constructor(private http: HttpClient, private router: Router,
    private injector: Injector) {
    // this.currentUser$ = null
  }

  getCurrentUserValue() {

    return this.currentUserSource.value;
  }

  getCurrentWishListValue() {

    return this.currentUWishListSource.value;
  }

  loadCurrentUser(token: string) {

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`)

    return this.http.get(this.baseUrl + 'account/getuser', { headers }).pipe(
      map((user: IUser) => {

        if (user) {

          // localStorage.setItem('token', user.token);
          this.currentUserSource.next(user)
        }
      })
    )
  }


  loadCustomerUsers(paginationData: any) {

    let params = new HttpParams();



    params = params.append("pageIndex", paginationData.pageIndex);
    params = params.append("pageSize", paginationData.pageSize);

    if (paginationData.search) {
      params = params.append("search", paginationData.search);
    }


    return this.http.get<IUserPagination>(this.baseUrl + "account/getallusers", {
      observe: "response",
      params: params

    }).pipe(
      map(response => {
        return response.body;
      })
    )
  }


  login(values: any) {

    return this.http.post(this.baseUrl + 'Account/login', values).pipe(
      map((user: IUser) => {

        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
          // Get user basket after a successful login
          this.injector.get(BasketService).initializeBasket();
          this.router.navigateByUrl('/');
        }
      })
    );
  }


  register(values: any) {

    return this.http.post(this.baseUrl + 'Account/register', values).pipe(
      map((response) => {
        this.assignRoleToUser(
          {
            RoleName: 'Customer',
            UserId: response['userId'],
            Action: 1
          }
        ).subscribe(() => {

          // console.log('role assigned');
        })
      })
    );
  }

  assignRoleToUser(values: any) {

    return this.http.post(this.baseUrl + 'Role/AssignRole', values).pipe(
      map((response) => {
        // console.log(response)
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
    localStorage.removeItem('token')

    // Remove local basket on user logout
    this.injector.get(BasketService).deleteLocalBasket();
  }


  changeProfileImage(values) {

    return this.http.post(this.baseUrl + 'Account', values).pipe(
      map((response) => {

        // console.log(response)
      })
    )
  }

  updateProfileData(values) {
    return this.http.put(this.baseUrl + 'Account/updateProfile', values).pipe(
      map((response) => {

        // console.log(response)
      })
    )
  }

  // getUserAddress(){
  //   return this.http.get<IAddress>(this.baseUrl + "/account/address");
  // }

  // updateUserAddress(address : IAddress){
  //   return this.http.put<IAddress>(this.baseUrl + "account/address", address);
  // }


  getWishList(token) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`)

    return this.http.get(this.baseUrl + 'Account/wishlist', { headers }).pipe(
      map((wishList: IWishList) => {

        if (wishList) {
          this.currentUWishListSource.next(wishList)
        }
      })
    )
  }

  addToWishList(token, productId: number) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`)
    let body = {
      productId: productId
    }
    return this.http.post(this.baseUrl + 'Account/wishlist/add', body, { headers })
      .subscribe({
        next: (response: IWishList) => {
          // console.log(response)
          this.currentUWishListSource.next(response)
        },
        error: (errors) => {
          // console.log(errors)
        }
      })
  }

  removeFromWishList(token, productId: number) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`)
    let body = {
      productId: productId
    }

    return this.http.post(this.baseUrl + 'Account/wishlist/remove', body, { headers })
      .subscribe({
        next: (response: IWishList) => {
          // console.log(response)
          this.currentUWishListSource.next(response)
        },
        error: (errors) => {
          // console.log(errors)
        }
      })
  }

  checkInWishList(productId) {

    for (let i = 0; i < this.currentUWishListSource.value.products.length ?? 0; i++) {

      if (productId == this.currentUWishListSource.value.products[i].id) {

        return true
      }
    }

    return false
  }
}
