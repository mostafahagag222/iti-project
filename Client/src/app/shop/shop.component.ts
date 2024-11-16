import { LabelType, Options } from '@angular-slider/ngx-slider';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { NavBarSearchService } from '../core/nav-bar/nav-bar-search.service';
import { IBrand } from '../shared/models/brand';
import { IColor } from '../shared/models/color';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  totalCount: number;
  shopParams: ShopParams = new ShopParams();
  sortOptions = [
    { name: "Name, A to Z", value: "nameAsc" },
    { name: "Name, Z to A", value: "nameDesc" },
    { name: "Price: Low to High", value: "priceAsc" },
    { name: "Price: High to Low", value: "priceDesc" }
  ];
  search$: Observable<string>;
  minValue = 0;
  maxValue = 100000;
  options: Options = {
    floor: 0,
    ceil: this.maxValue,
    enforceRange: true,
    translate: (value: number, label: LabelType): string => {
      switch (label) {
        case LabelType.Low:
          return "$" + value;
        case LabelType.High:
          return "$" + value;
        default:
          return "$" + value;
      }
    }
  };

  constructor(private shopService: ShopService, private searchService: NavBarSearchService) {



  }

  ngOnInit(): void {
    this.search$ = this.searchService.search$;
    this.search$.subscribe({
      next: res => { this.shopParams.search = res; this.shopParams.pageIndex = 1; this.getProducts(); }
    });
    this.getBrands();
    this.getTypes();
    this.getTypesCount();
    this.getProducts();
    this.getColors();
    this.getMaxPrice();
  }


  getProducts() {

    this.shopService.getProducts(this.shopParams).subscribe(res => {
      this.products = res.data;
      this.shopParams.pageIndex = res.pageIndex;
      this.shopParams.pageSize = res.pageSize;
      this.totalCount = res.count;
    }, err => {
      console.log(err);
    })
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: res => { this.brands = [{ id: 0, name: "All", origin: "Any" }, ...res] },
      error: err => { console.log(err); }
    })
  }

  getTypes() {
    this.shopService.getTypes().subscribe({
      next: res => { this.types = [{ id: 0, name: "All" }, ...res]; },
      error: err => { console.log(err); }
    })
  }

  colors: IColor[];
  getColors() {
    this.shopService.getColors().subscribe({
      next: res => { this.colors = res },
      error: err => { console.log(err); }
    })
  }
  typesCount: any;
  getTypesCount() {
    this.shopService.getTypesCount().subscribe({
      next: res => { this.typesCount = res; this.typesCountResult = this.typesWithCount() },
      error: err => { console.log(err); }
    })
  }

  typesCountResult: any;
  typesWithCount() {
    const map = new Map();
    this.types?.forEach(item => map.set(item.id, item));
    this.typesCount.forEach(item => map.set(item.id, { ...map.get(item.id), ...item }));
    let mergedArr = Array.from(map.values());
    return mergedArr;
    //return this.types.map((item, i) => Object.assign({}, item, this.typesCount[i]));
  }

  getMaxPrice() {
    this.shopService.getMaxPrice().subscribe({
      next: res => {
        this.maxValue = res;
        const newOptions: Options = Object.assign({}, this.options);
        newOptions.ceil = this.maxValue;
        this.options = newOptions;
      },
      error: err => { console.log(err); }
    })
  }

  /////////////////////
  priceRangeChanged() {
    this.shopParams.priceFrom = this.minValue;
    this.shopParams.priceTo = this.maxValue;
    this.shopParams.pageIndex = 1;
    this.getProducts();
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.shopParams.pageIndex = 1;
    this.getProducts();
  }

  onColorSelected(color: string) {
    this.shopParams.color = color;
    this.shopParams.pageIndex = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.shopParams.pageIndex = 1;
    this.getProducts();
  }

  onSortOptionChanged(event: any) {
    this.shopParams.sort = event.target.value;
    this.getProducts();
  }

  sortNameAsc() {
    this.shopParams.sort = "nameAsc";
    this.getProducts();
  }

  sortNameDesc() {
    this.shopParams.sort = "nameDesc";
    this.getProducts();
  }

  sortPriceAsc() {
    this.shopParams.sort = "priceAsc";
    this.getProducts();
  }

  sortPriceDesc() {
    this.shopParams.sort = "priceDesc";
    this.getProducts();
  }

  onPageChanged(event: any) {
    if (this.shopParams.pageIndex != event.page) {
      this.shopParams.pageIndex = event.page;
      this.getProducts();
    }
  }

  clearFilters() {
    this.shopParams = new ShopParams();
    this.getProducts();
    this.getMaxPrice();
  }
}
