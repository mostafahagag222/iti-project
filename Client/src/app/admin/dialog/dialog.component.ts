import { Component, OnInit } from '@angular/core';
import { FormGroup,FormBuilder,Validator, Validators, FormControl } from '@angular/forms';
import { ProductService } from '../services/product.service';

import { MatDialogRef } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';


@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent implements OnInit {

  images = [];
  myFiles:string [] = [];
  actionBtn :string="Save";

  hideFileInput=true;

  productForm !: FormGroup;
  constructor(private formBuilder:FormBuilder,
    private dialogRef:MatDialogRef<DialogComponent>
    ,   @Inject(MAT_DIALOG_DATA) public data: any

    ) { }

  ngOnInit(): void {

    this.productForm=this.formBuilder.group({

      product:
      this.formBuilder.group({

      Name:['',Validators.required],
      Description:['',Validators.required],
      Price:['',Validators.required],
      UnitsInStock:['',Validators.required],

      ManufactureDate:['',Validators.required],
      Height:['',Validators.required],
      Width:['',Validators.required],
      Length:['',Validators.required],
      Weight:['',Validators.required],
      Color:['',Validators.required],

      ProductTypeId:['',Validators.required],
      ProductBrandId:['',Validators.required],

      }),
      file: ['',Validators.required],
      files: ['',Validators.required]
    });

    if(this.data.product){

      this.productForm.get("product").get("Name").setValue(this.data.product.name);
      this.productForm.get("product").get("Description").setValue(this.data.product.description);
      this.productForm.get("product").get("Price").setValue(this.data.product.price);
      this.productForm.get("product").get("UnitsInStock").setValue(this.data.product.unitsInStock);

      this.productForm.get("product").get("ManufactureDate").setValue(this.data.product.manufactureDate);
      this.productForm.get("product").get("Height").setValue(this.data.product.height);
      this.productForm.get("product").get("Width").setValue(this.data.product.width);
      this.productForm.get("product").get("Length").setValue(this.data.product.length);
      this.productForm.get("product").get("Weight").setValue(this.data.product.weight);
      this.productForm.get("product").get("Color").setValue(this.data.product.color);

      const type=this.data.types.find(
        (obj)=>{
          return obj.name==this.data.product.productType;
        }
      );

      const brand=this.data.brands.find(
        (obj)=>{
          return obj.name == this.data.product.productBrand;
        }
      );

      this.productForm.get("product").get("ProductTypeId").setValue(type.id);
      this.productForm.get("product").get("ProductBrandId").setValue(brand.id);

      if(this.data.action=="edit"){
        this.actionBtn="Update";
      }
      if(this.data.action=="show"){
        this.hideFileInput=false;
         this.images=this.data.product.pictures;
      }

    }


  }

  onFileChange(event) {

    for (var i = 0; i < event.target.files.length; i++) {

      var reader = new FileReader();

      reader.onload = (event:any) => {
        console.log(event.target.result);
         this.images.push(event.target.result);
      }
      reader.readAsDataURL(event.target.files[i]);

      this.myFiles.push(event.target.files[i]);
      this.productForm.get("files").setValue(this.myFiles);

  }



  }
  get f(){
    return this.productForm.controls;
  }

  public makeAction(){

    const formData = new FormData();
    formData.append('product', JSON.stringify(this.productForm.get('product').value));

    for (var i = 0; i < this.myFiles.length; i++) {


      console.log(this.myFiles[i]);


      formData.append("files", this.myFiles[i]);
    }

    if(this.data.action=="add"){
      this.dialogRef.close({event:'add',data:formData});
    }
    if(this.data.action=="edit"){
      this.dialogRef.close({event:'edit',data:formData});
    }




  }






}
