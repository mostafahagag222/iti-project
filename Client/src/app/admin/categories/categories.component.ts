import { Component, OnInit,ElementRef,ViewChild  } from '@angular/core';
import {MatDialog} from '@angular/material/dialog';


import { PageEvent} from '@angular/material/paginator';

import { IType } from 'src/app/shared/models/productType';
import { TypeService } from '../services/type.service';
import { BrandDialogComponent } from '../brand-dialog/brand-dialog.component';
import { TypeDialogComponent } from '../type-dialog/type-dialog.component';


@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {

  pageIndex:number;
  pageSize:number;
  search:string;
  count:number;
  types: IType[];
  constructor(private dialog:MatDialog,private typeService:TypeService) { }

  ngOnInit(): void {

    this.pageIndex=1;
    this.pageSize=6;

    this.getTypes();

  }
  @ViewChild("search", { static: false }) searchTerm: ElementRef;


  AddDialog(){
    const dialogRef =this.dialog.open(TypeDialogComponent, {

      width:'35%',
      data: {
        action:"add"
      }
  });

  dialogRef.afterClosed().subscribe(result => {
    if(result.event=='add')
      this.addType(result.data);


  });

  }

  addType(type:any){
    this.typeService.postType(type)
      .subscribe(

        {
          next:(res)=>{

            alert("Type added successfuly");
            this.getTypes();

          },
          error:(err)=>{
            console.log(err);
          }
        }

      )

  }

  getTypes(){
    this.typeService.getTypesPagination({pageIndex:this.pageIndex,pageSize:this.pageSize,search:this.search}).subscribe({
      next:(res)=>{

        this.types = res.data;
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

  updateType(id:number,data:any){
    this.typeService.updateType(id,data)
        .subscribe(

          {
            next:(res)=>{

              alert("Type updated successfuly");
              this.getTypes();

            },
            error:(err)=>{
              console.log(err);
            }
          }

        )
  }

  deleteType(id:number){

    this.typeService.deleteType(id)
    .subscribe({
      next:(res)=>{
        alert("Product Deleted Successfully")
        this.getTypes();
      },
      error:(err)=>{
        console.log(err);
      }


    })
  }

  onPageChanged(event: PageEvent){
    this.pageIndex = event.pageIndex+1;
    this.getTypes();

  }

  editType(type:any){

    const dialogRef =this.dialog.open(TypeDialogComponent, {

      width:'35%',
      data: {
        Type:type,
        action:"edit"

      }
  });
  dialogRef.afterClosed().subscribe(result => {
    if(result.event=='edit')
      result.data.id=type.id;
      this.updateType(type.id,result.data);


  })

  }

  confrimDelete(id:number) {
    if(confirm("Are you sure to delete ")) {
      this.deleteType(id);
    }
  }
  onSearch(){
    this.search=this.searchTerm.nativeElement.value;
    this.getTypes();

  }

}
