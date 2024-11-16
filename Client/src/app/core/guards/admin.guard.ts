import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  routeURL: string;
  constructor(private accountService: AccountService, private router: Router) {
    this.routeURL = this.router.url;
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    // here we check if user is logged in or not
    // the authService returs user object, or
    // it returns undefined/null when user is not logged in

    // SINCE OUR 'authService.user' IS OF TYPE 'Observable'
    // WE MUST USE 'subscribe' TO GET VALUE OF 'user'
    return new Promise((resolve, reject) => {
      this.accountService.currentUser$.subscribe((user) => {
        // console.log('user', user);
        // check if user is not loggedIn(!user)
        // and routeURL !== '/login'
        // console.log(user)
        // console.log(this.routeURL)
        if (user) {
          if (user.roles.includes('Admin')) {
            // // assign '/login' in 'routeURL' to
            // // avoid get into infinite loop
            // this.routeURL = 'account/login';
            // // when the user is not logged in,
            // // instead of just returning false
            // // inject router and redirect to '/login' or any other view
            // this.router.navigate(['account/login']);
            return resolve(true);
          }
        }
        else {
          // re-assign current route URL to 'routeURL'
          // when the user is logged in
          this.routeURL = '/shop';
          this.router.navigate(['shop/']);
          // just return true - if user is logged in
          return resolve(false);
        }
      });
    });
  }
}
