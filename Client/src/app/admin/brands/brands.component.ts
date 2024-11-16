import { Component, OnInit,ElementRef,ViewChild  } from '@angular/core';
import {MatDialog} from '@angular/material/dialog';


import { PageEvent} from '@angular/material/paginator';

import { IBrand } from 'src/app/shared/models/brand';

import { BrandService } from '../services/brand.service';
import { BrandDialogComponent } from '../brand-dialog/brand-dialog.component';

@Component({
  selector: 'app-brands',
  templateUrl: './brands.component.html',
  styleUrls: ['./brands.component.scss']
})
export class BrandsComponent implements OnInit {
  pageIndex:number;
  pageSize:number;
  search:string;
  count:number;
  brands: IBrand[];

  constructor(private dialog:MatDialog,private brandService:BrandService) { }

  ngOnInit(): void {
    this.pageIndex=1;
    this.pageSize=6;
    this.getBrands();
  }
  @ViewChild("search", { static: false }) searchTerm: ElementRef;

  AddDialog(){
    const dialogRef =this.dialog.open(BrandDialogComponent, {

      width:'35%',
      data: {
        action:"add"
      }
  });

  dialogRef.afterClosed().subscribe(result => {
    if(result.event=='add')
      this.addBrand(result.data);


  });

  }

  addBrand(brand:any){
    this.brandService.postBrand(brand)
      .subscribe(

        {
          next:(res)=>{

            alert("Brand added successfuly");
            this.getBrands();

          },
          error:(err)=>{
            console.log(err);
          }
        }

      )

  }
  getBrands(){
    this.brandService.getBrandsPagination({pageIndex:this.pageIndex,pageSize:this.pageSize,search:this.search}).subscribe({
      next:(res)=>{

        this.brands = res.data;
        this.pageIndex=res.pageIndex;
        this.pageSize=res.pageSize;
        this.count=res.count;
        console.log(res);
      },
      error:(err)=>{
        console.log(err);
      }


    })

  }

  updateBrand(id:number,data:any){
    this.brandService.updateBrand(id,data)
        .subscribe(

          {
            next:(res)=>{

              alert("Brand updated successfuly");
              this.getBrands();

            },
            error:(err)=>{
              console.log(err);
            }
          }

        )
  }

  deleteBrand(id:number){

    this.brandService.deleteBrand(id)
    .subscribe({
      next:(res)=>{
        alert("Product Deleted Successfully")
        this.getBrands();
      },
      error:(err)=>{
        console.log(err);
      }


    })
  }

  onPageChanged(event: PageEvent){
    this.pageIndex = event.pageIndex+1;
    this.getBrands();

  }

  editBrand(brand:any){

    const dialogRef =this.dialog.open(BrandDialogComponent, {

      width:'35%',
      data: {
        brand:brand,
        action:"edit"

      }
  });
  dialogRef.afterClosed().subscribe(result => {
    if(result.event=='edit')
      result.data.id=brand.id;
      this.updateBrand(brand.id,result.data);


  })

  }

  confrimDelete(id:number) {
    if(confirm("Are you sure to delete ")) {
      this.deleteBrand(id);
    }
  }

  onSearch(){
    this.search=this.searchTerm.nativeElement.value;
    this.getBrands();

  }
}
