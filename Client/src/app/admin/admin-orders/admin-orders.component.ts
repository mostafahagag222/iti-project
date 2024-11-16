import { LabelType, Options } from '@angular-slider/ngx-slider';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { IOrder, orderParams } from 'src/app/shared/models/order';
import { IOrdersPagination } from 'src/app/shared/models/pagination';
import { DialogOrderComponent } from '../dialog-order/dialog-order.component';
import { AdminOrdersService } from './admin-orders.service';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {


  orders : IOrder[];
  length : number;
  totalCount: number;
  searchOption = [{name: "Pending", value : "0"}, {name:"Confirmed", value:"1"}];
  sortOptions = [{name: "Lower Cost To Higher", value: "subTotalAsc"}, {name: "Higher Cost To Lower", value: "subTotalDesc"}];
  orderParamsToSend : orderParams;
  pageSize : number = 6;




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
  orderDetails: IOrder;


  constructor(private adminOrdersService : AdminOrdersService, public dialog : MatDialog, private toastr : ToastrService) { }

  ngOnInit(): void {
    this.orderParamsToSend = new orderParams();
    this.orderParamsToSend.pageIndex = 1;
    this.orderParamsToSend.pageSize = 6;
    this.orderParamsToSend.sort = "subTotalAsc";
    this.orderParamsToSend.search;
    this.getMaxPrice();
    this.orderParamsToSend.subTotalFrom = 0;
    this.getOrders();
  }

  private getOrders() {

    this.adminOrdersService.getAllOrders(this.orderParamsToSend).subscribe(
      {
        next : (response : IOrdersPagination) => {
          this.orders = response.data;
          this.orderParamsToSend.pageIndex = response.pageIndex;
          this.orderParamsToSend.pageSize = response.pageSize;
          this.totalCount = response.count;
        },
        error : err => console.log(err)
      }
    )

  }

  priceRangeChanged() {
    this.orderParamsToSend.subTotalFrom = this.minValue;
    this.orderParamsToSend.subTotalTo = this.maxValue;
    this.orderParamsToSend.pageIndex = 1;
    this.getOrders();
  }

  getMaxPrice() {
    this.adminOrdersService.getMaxPrice().subscribe({
      next: res => {
        this.maxValue = res;
        const newOptions: Options = Object.assign({}, this.options);
        newOptions.ceil = this.maxValue;
        this.options = newOptions;
        console.log(this.maxValue);
      },
      error: err => { console.log(err); }
    })
  }

  confirmAllOrders() {
      this.adminOrdersService.confirmAllOrders().subscribe(
        {
          next: ( data : IOrdersPagination[]) => {
            this.toastr.success("All orders confirmed successfully");
            this.getOrders()
          }
        }
      );
  }

  showDetails(orderId : number) {

    console.log(this.orders.find(o => o.id == orderId));

    const dialogRef =this.dialog.open(DialogOrderComponent, {

      width:'35%',
      data: {
        order: this.orders.find(o => o.id == orderId),
        action:"show",
        itemsCount:this.orders.find(o => o.id == orderId).orderItems.length
      }
  });
}

  confirmOrder(order : IOrder)  {
    this.adminOrdersService.confirmOrder(order.id, order).subscribe({
      next: ( data : IOrder) => {
        this.toastr.success("Order confirmed");
      },
      error : err => this.toastr.error("Error! order isn't confirmed")
    })
  }

  confrimDelete(id:number) {
    if(confirm("Are you sure to delete ")) {
      this.deleteOrder(id);
    }
  }

  private deleteOrder(id:number){
    this.adminOrdersService.deleteOrder(id).subscribe({
      next: (data) => {
          this.toastr.success("Order deleted");
          this.getOrders();
        }
    });

  }

  onSortSelected(e : Event) {
    this.orderParamsToSend.sort = (e.target as HTMLSelectElement).value;
    this.orderParamsToSend.pageIndex = 1;
    this.getOrders();
  }

  onFilterSelected(e : Event) {
    let filterValue = (e.target as HTMLSelectElement).value;
    if(filterValue === "3")
    {
      this.clearFilters();
    }
    else {
      this.orderParamsToSend.search = filterValue;
      this.orderParamsToSend.pageIndex = 1;
    }
    console.log(filterValue);
    this.getOrders();
  }

  onPageChanged(event: PageEvent) {
    this.orderParamsToSend.pageIndex = event.pageIndex+1;
    this.getOrders();
  }

  private clearFilters() {
    this.orderParamsToSend = new orderParams();
    this.orderParamsToSend.pageIndex = 1;
    this.orderParamsToSend.pageSize = 6;
    this.getMaxPrice();
    this.orderParamsToSend.subTotalFrom = 0;
    this.getOrders();
  }

}
