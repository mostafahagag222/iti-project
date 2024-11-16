import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { SidenavComponent } from './sidenav/sidenav.component';
import { ProductsComponent } from './products/products.component';
import { CategoriesComponent } from './categories/categories.component';
import { BodyComponent } from './body/body.component';
import { BrandsComponent } from './brands/brands.component';

import { UsersComponent } from './users/users.component';
import { AdminOrdersComponent } from './admin-orders/admin-orders.component';

import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogComponent } from './dialog/dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';

import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';

import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { CarouselModule } from 'ngx-bootstrap/carousel';

import { MatTableModule } from '@angular/material/table';

import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';

import { BrandDialogComponent } from './brand-dialog/brand-dialog.component';
import { TypeDialogComponent } from './type-dialog/type-dialog.component';
import { ShopModule } from '../shop/shop.module';
import { DialogOrderComponent } from './dialog-order/dialog-order.component';

import { NgChartsModule } from 'ng2-charts';



@NgModule({
  declarations: [


    AdminHomeComponent,
    DashboardComponent,
    SidenavComponent,
    ProductsComponent,
    CategoriesComponent,
    BodyComponent,
    BrandsComponent,
    UsersComponent,
    AdminOrdersComponent,
    DialogComponent,

    BrandDialogComponent,
      TypeDialogComponent,
      DialogOrderComponent,

  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    CarouselModule.forRoot(),
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    ShopModule,
    NgChartsModule

  ]
})
export class AdminModule { }
