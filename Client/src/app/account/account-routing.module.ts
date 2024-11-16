import { WishListGuard } from './../core/guards/wishlist.guard';
import { WishListResolver } from '../core/resolvers/wishlist.resolver';
import { WishlistComponent } from './wishlist/wishlist.component';
import { AuthAccessGuard } from './../core/guards/AuthAccess.guard';
import { ProfileGuard } from './../core/guards/Profile.guard';
import { ProfileComponent } from './profile/profile.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule, CanActivate } from '@angular/router';


import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { UserResolver } from '../core/resolvers/user.resolver';

const routes: Routes = [
  { path: 'login', component: LoginComponent, canActivate: [AuthAccessGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [AuthAccessGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [ProfileGuard] },
  { path: 'wishlist', component: WishlistComponent, },
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
