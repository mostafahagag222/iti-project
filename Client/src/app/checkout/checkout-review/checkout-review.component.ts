import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { productViewModel } from 'src/app/shared/viewmodels/product-viewmodel';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent implements OnInit {
  @Input() appStepper : CdkStepper;
  basket$:Observable<IBasket>;
  checkoutForm: FormGroup;

  basketProducts : any[];

  constructor(private basketService: BasketService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.basketService.basket$.subscribe(
      {
        next : data => {
          this.basketProducts = data.basketItems;
        }
      }
    )
  }

  createPaymentIntent(){
    return this.basketService.createPaymentIntent().subscribe({
      next: (response: any) => {
        this.appStepper.next();
      },
      error: (err) => {
        console.log(err.message);
      }
    })
  }

}
