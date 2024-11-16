import { Component, OnInit,ElementRef,ViewChild } from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import { DialogComponent } from '../dialog/dialog.component';


import { ProductService } from '../services/product.service';

import { IProduct } from 'src/app/shared/models/product';
import { ShopParams } from 'src/app/shared/models/shopParams';

import { PageEvent} from '@angular/material/paginator';

import {MatTableDataSource} from '@angular/material/table';
import { IBrand } from 'src/app/shared/models/brand';
import { IType } from 'src/app/shared/models/productType';
import { IOrder } from 'src/app/shared/models/order';




@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  products: IProduct[];
  shopParams: ShopParams = new ShopParams();
  totalCount: number;




  displayedColumns: string[] = ['Name', 'Price', 'Quantity', 'Product Type','Product Brand'


];
  dataSource: MatTableDataSource<any>;


  brands: IBrand[];
  types: IType[];



  constructor(private dialog:MatDialog,private productService:ProductService) { }

  ngOnInit(): void {
    this.shopParams.pageIndex=1;
    this.shopParams.pageSize=6;
    this.getProducts();


    this.getBrands();
    this.getTypes();
  }
  @ViewChild("search", { static: false }) searchTerm: ElementRef;



  confrimDelete(id:number) {
    if(confirm("Are you sure to delete ")) {
      this.deleteProduct(id);
    }
  }


  openDialog() {
    const dialogRef =this.dialog.open(DialogComponent, {

      width:'35%',
      data: {
        types: this.types,
        brands:this.brands,
        action:"add"
      }
  });

  dialogRef.afterClosed().subscribe(result => {
    if(result.event=='add')
      this.addProduct(result.data);


  });



}

editProduct(product:any){
  const dialogRef =this.dialog.open(DialogComponent, {

    width:'35%',
    data: {
      types: this.types,
      brands:this.brands,
      product:product,
      action:"edit"

    }
});
dialogRef.afterClosed().subscribe(result => {
  if(result.event=='edit')
    this.updateProduct(product.id,result.data);


});

}



showDetails(product:any){
  const dialogRef =this.dialog.open(DialogComponent, {

    width:'35%',
    data: {
      types: this.types,
      brands:this.brands,
      product:product,
      action:"show"

    }
});
}
/*

export interface IOrder{
  id : number;
  buyerEmail : string;
  orderDate : string;
  shipToAddress : IAddress;
  deliveryMethod : string;
  shippingPrice : number;
  orderItems : IOrderItem[];
  subtotal : number;
  total : number;
  status : string;
}

export interface IOrderItem{
  productId : number;
  productName : string;
  price : number;
  quantity : number;
}


export class orderParams {
  pageIndex: number;
  pageSize: number;
  subTotalFrom : number;
  subTotalTo : number;
  sort : string;
  search: string;
}

*/



addProduct(data:FormData){

  this.productService.postProduct(data)
      .subscribe(

        {
          next:(res)=>{

            alert("Product added successfuly");
            this.getProducts();

          },
          error:(err)=>{
            console.log(err);
          }
        }

      )

}

getProducts() {

  this.productService.getProducts(this.shopParams).subscribe(res => {
    this.products = res.data;
    this.shopParams.pageIndex = res.pageIndex;
    this.shopParams.pageSize = res.pageSize;
    this.totalCount = res.count;

  }, err => {
    console.log(err);
 })
}


deleteProduct(id:number){

  this.productService.deleteProduct(id)
  .subscribe({
    next:(res)=>{
      alert("Product Deleted Successfully")
      this.getProducts();
    },
    error:(err)=>{
      console.log(err);
    }


  })
}

updateProduct(id:number,data:FormData){
  this.productService.updateProduct(id,data)
      .subscribe(

        {
          next:(res)=>{

            alert("Product updated successfuly");
            this.getProducts();

          },
          error:(err)=>{
            console.log(err);
          }
        }

      )
}


onPageChanged(event: PageEvent) {

  this.shopParams.pageIndex = event.pageIndex+1;
  this.getProducts();
}

getBrands() {
  this.productService.getBrands().subscribe({
    next: res => { this.brands = res },
    error: err => { console.log(err); }
  })
}

getTypes() {
  this.productService.getTypes().subscribe({
    next: res => { this.types = res; },
    error: err => { console.log(err); }
  })
}
onSearch(){

  this.shopParams.search=this.searchTerm.nativeElement.value;
  this.getProducts();

}


}
