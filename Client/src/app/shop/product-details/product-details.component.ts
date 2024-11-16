import { Observable } from 'rxjs';
import { IReview } from './../../shared/models/review';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ReviewService } from '../review.service';
import { ShopService } from '../shop.service';
import { IProductReviews } from 'src/app/shared/models/productReviews';
import { IUser } from 'src/app/shared/models/user';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  product: IProduct;
  productReviews: IProductReviews;
  currentUser: IUser;
  quantity: number = 1;
  reviewBody: string;
  Reviews$: Observable<IProductReviews>


  ratingList: boolean[] = [true, true, true, true, true];
  rating: number = 0;


  constructor(private shopService: ShopService, private accountService: AccountService, private reviewService: ReviewService, private activeRoute: ActivatedRoute, private breadCrum: BreadcrumbService) {
    this.breadCrum.set("@productDetails", " ");
    this.Reviews$ = this.reviewService.Reviews$;
  }

  ngOnInit(): void {
    this.loadProduct();

  }

  loadProduct() {
    this.shopService.getProduct(+this.activeRoute.snapshot.paramMap.get("id")).subscribe(product => {

      this.product = product;
      this.breadCrum.set("@productDetails", product.name);

      this.reviewService.getReviews(product.id)
        .subscribe({
          next: ((reviews: IProductReviews) => {
            this.productReviews = reviews
            this.currentUser = this.accountService.getCurrentUserValue();
            console.log(reviews)
          }),
          error: (errors => {
            console.log(errors.error)
          })
        })
    }, error => {
      console.log(error);
    }

    );
  }

  increaseQuantity() {
    if (this.quantity + 1 > this.product.unitsInStock) {
      this.quantity = this.product.unitsInStock;
    }
    else {
      this.quantity++;
    }
  }
  decreaseQuantity() {

    if (this.quantity - 1 == 0) {
      this.quantity = 1;
    }
    else {
      this.quantity--;
    }

  }

  setStar(data: any) {
    this.rating = data + 1;
    for (var i = 0; i <= 4; i++) {
      if (i <= data) {
        this.ratingList[i] = false;
      }
      else {
        this.ratingList[i] = true;
      }
    }
    console.log(this.rating);
  }


  submitReview() {

    this.reviewService.addReview(localStorage.getItem('token'), this.product.id, this.reviewBody, this.rating)
      .subscribe({
        next: ((response) => {
        }
        ),
        error: ((errors) => {
          console.log(errors)
        })
      })
  }

}
