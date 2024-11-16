import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { orderParams } from 'src/app/shared/models/order';

@Component({
  selector: 'app-dialog-order',
  templateUrl: './dialog-order.component.html',
  styleUrls: ['./dialog-order.component.scss']
})
export class DialogOrderComponent implements OnInit {

  orderForm !: FormGroup;
  // items = this.data.order.orderItems;

  constructor(private formBuilder:FormBuilder,
    private dialogRef:MatDialogRef<DialogOrderComponent>,
            @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {

    this.orderForm =  this.formBuilder.group({

      id : ['', Validators.required],
      buyerEmail : ['', Validators.required],
      orderDate : ['', Validators.required],

      shipToAddress : this.formBuilder.group({
        firstName : ['',Validators.required],
        lastName : ['',Validators.required],
        street : ['',Validators.required],
        city : ['',Validators.required],
        state : ['',Validators.required],
        zipcode : ['',Validators.required],
      }),

      deliveryMethod : ['',Validators.required],
      shippingPrice : ['',Validators.required],

      orderItems : this.formBuilder.array([this.createSection()], Validators.required),

      subtotal : ['',Validators.required],
      total : ['',Validators.required],
      status : ['',Validators.required],

    });

    let orderData = this.data.order;

    if(this.data.order){
      this.orderForm.get('id').setValue(orderData.id);
      this.orderForm.get('buyerEmail').setValue(orderData.buyerEmail);
      this.orderForm.get('orderDate').setValue(orderData.orderDate);
      this.orderForm.get('shipToAddress').get('firstName').setValue(orderData.shipToAddress.firstName);
      this.orderForm.get('shipToAddress').get('lastName').setValue(orderData.shipToAddress.lastName);
      this.orderForm.get('shipToAddress').get('street').setValue(orderData.shipToAddress.street);
      this.orderForm.get('shipToAddress').get('city').setValue(orderData.shipToAddress.city);
      this.orderForm.get('shipToAddress').get('state').setValue(orderData.shipToAddress.state);
      this.orderForm.get('shipToAddress').get('zipcode').setValue(orderData.shipToAddress.zipcode);
      this.orderForm.get('deliveryMethod').setValue(orderData.deliveryMethod);
      this.orderForm.get('shippingPrice').setValue(orderData.shippingPrice);

      this.orderForm.get('subtotal').setValue(orderData.subtotal);
      this.orderForm.get('total').setValue(orderData.total);
      this.orderForm.get('status').setValue(orderData.status);

      for(let i = 0; i < this.data.itemsCount; i++)
      {
        this.orderForm.get('orderItems')[i].get('productId').setValue(orderData.orderItems[i].productId);
        this.orderForm.get('orderItems')[i].get('productName').setValue(orderData.orderItems[i].productName);
        this.orderForm.get('orderItems')[i].get('price').setValue(orderData.orderItems[i].price);
        this.orderForm.get('orderItems')[i].get('quantity').setValue(orderData.orderItems[i].quantity);
      }

    }

    console.log(this.orderForm);
  }

  get items(){
    return this.orderForm.get('orderItems') as FormArray;
  }



  private createSection() {

    return this.formBuilder.group({
      productId : ['',Validators.required],
      productName : ['',Validators.required],
      price : ['',Validators.required],
      quantity : ['',Validators.required]
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

*/


}
