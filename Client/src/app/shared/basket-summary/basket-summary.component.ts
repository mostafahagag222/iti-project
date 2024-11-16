import { outputAst } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketRoutingModule } from 'src/app/basket/basket-routing.module';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketItem } from '../models/basket';
import { IProduct } from '../models/product';
import { productViewModel } from '../viewmodels/product-viewmodel';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {

  basket$ : Observable<IBasket>;

  @Input() basketProducts : productViewModel[];

  basketItems : IBasketItem[];

  @Output() decrement : EventEmitter<productViewModel> = new EventEmitter<productViewModel>();
  @Output() increment : EventEmitter<productViewModel> = new EventEmitter<productViewModel>();
  @Output() remove : EventEmitter<productViewModel> = new EventEmitter<productViewModel>();

  @Input() isBasket = true;

  constructor(private basketService : BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.basket$.subscribe(
      {
        next : (data) => {
          console.log(data.basketItems);
          this.basketItems = data.basketItems;
        }
      }
    )
  }

  decrementItemQuantity(item : productViewModel)
  {
    this.decrement.emit(item);
  }

  incrementItemQuantity(item : productViewModel)
  {
    this.increment.emit(item);
  }

  removeBasketItem(item : productViewModel)
  {
    this.remove.emit(item);
  }

}
