import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OwlOptions } from 'ngx-owl-carousel-o';
import { IProduct } from '../shared/models/product';
import { HomeService } from './home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private router: Router, private homeService: HomeService) { }

  trendyProducts: IProduct[];
  bannerOptions: OwlOptions = {

    loop: true,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: false,
    dots: true,
    dotsEach: true,
    center: true,
    navSpeed: 600,
    nav: false,
    items: 1,

  };
  customOptions: OwlOptions = {
    loop: true,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: false,
    dots: true,
    dotsEach: true,
    center: true,
    navSpeed: 600,
    nav: false,
    responsive: {
      0: {
        items: 1
      },
      400: {
        items: 2
      },
      760: {
        items: 3
      },
      1000: {
        items: 4
      }
    }
  }


  ngOnInit(): void {
    this.getTrendyProducts();
  }


  getTrendyProducts() {
    this.homeService.getTrendyProducts().subscribe({
      next: res => this.trendyProducts = res,
      error: err => console.log(err)
    })
  }



  ////////
  goToShop() {
    this.router.navigateByUrl("shop");
  }

  routeToProduct(id: number) {
    this.router.navigateByUrl(`shop/${id}`);
  }

}
