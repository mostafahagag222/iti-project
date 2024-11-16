import { AdminGuard } from './core/guards/admin.guard';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { AuthAccessGuard } from './core/guards/AuthAccess.guard';
import { ShopComponent } from './shop/shop.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AdminHomeComponent } from './admin/admin-home/admin-home.component';
import { AboutComponent } from './about/about.component';

const routes: Routes = [
  {
    path: '', component: MainLayoutComponent, children: [
      {
        path: 'basket', loadChildren: () => import('./basket/basket.module')
          .then(mod => mod.BasketModule), data: { breadcrumb: "Basket" }
      },

      {
        path: 'checkout', loadChildren: () => import('./checkout/checkout.module')
          .then(mod => mod.CheckoutModule), data: { breadcrumb: "Checkout" }
      },
      {
        path: 'orders',
        loadChildren: () => import('./orders/orders.module')
          .then(mod => mod.OrdersModule), data: { breadcrumb: 'Orders' }
      },
      {
        path: "", component: HomeComponent, data: { breadcrumb: "Home" }
      },
      {
        path: "about", component: AboutComponent, data: { breadcrumb: "About" }
      },
      {
        path: "shop",
        loadChildren: () => import("./shop/shop.module")
          .then(mod => mod.ShopModule), data: { breadcrumb: "Shop" }
      },
      {
        path: "account",
        loadChildren: () => import("./account/account.module")
          .then(mod => mod.AccountModule), data: { breadcrumb: { skip: true } }
      },
    ]
  }
  ,

  {
    path: "admin",
    canActivate: [AdminGuard],
    component: AdminHomeComponent,
    loadChildren: () => import("./admin/admin.module")
      .then(mod => mod.AdminModule), data: { breadcrumb: { skip: true } }
  },


  { path: "**", redirectTo: "", pathMatch: "full" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
