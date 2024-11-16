import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/shared/models/basket';
import { AccountService } from './../../account/account.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { IUser } from 'src/app/shared/models/user';
import { NavBarSearchService } from './nav-bar-search.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})

export class NavBarComponent implements OnInit {
  basket$: Observable<IBasket>;
  currentUser$: Observable<IUser>;

  constructor(private basketService: BasketService, private accountService: AccountService,
    private navBarService: NavBarSearchService, private router: Router) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.currentUser$ = this.accountService.currentUser$
  }

  @ViewChild("search", { static: false }) searchTerm: ElementRef;

  logOut() {

    this.accountService.logout();
  }


  onSearch() {
    this.router.navigateByUrl("/shop");
    this.navBarService.searchSource.next(this.searchTerm.nativeElement.value);
  }

  getUserOrders(){
    this.router.navigateByUrl("/orders");
  }
}

