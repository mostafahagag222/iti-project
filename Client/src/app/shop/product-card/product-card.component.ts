import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { IWishList } from 'src/app/shared/models/wishlist';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent implements OnInit {
  @Input() product: IProduct;
  wishListItems: Observable<IWishList>;
  currentWishList$: any;
  inWishList: boolean = false;
  constructor(private basketService: BasketService, private accountService: AccountService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    if (localStorage.getItem('token') !== null) {
      this.wishListItems = this.accountService.WishList$
      this.checkInWishList()
    }

  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.product);
  }

  addToWishList() {
    let token = localStorage.getItem('token')
    this.accountService.loadCurrentUser(token)
      .subscribe({
        next: ((response) => {

          this.accountService.addToWishList(token, this.product.id)
          this.inWishList = true
        }),
        error: ((error) => {
          console.log('you are not logged in')
        })
      })
  }

  removeFromWishList() {

    let token = localStorage.getItem('token')
    this.accountService.loadCurrentUser(token)
      .subscribe({
        next: ((response) => {

          this.accountService.removeFromWishList(token, this.product.id)
          this.inWishList = false
        }),
        error: ((error) => {
          console.log('you are not logged in')
        })
      })
  }

  checkInWishList() {
    this.accountService.loadCurrentUser(localStorage.getItem('token'))
      .subscribe({
        next: ((response) => {


          this.inWishList = this.accountService.checkInWishList(this.product.id)
        }),
        error: ((error) => {
          console.log('you are not logged in')
        })
      })
    // if ( !== null) {

    //   this.inWishList = this.accountService.checkInWishList(this.product.id)
    //   console.log(this.inWishList)
    // }
  }
}
