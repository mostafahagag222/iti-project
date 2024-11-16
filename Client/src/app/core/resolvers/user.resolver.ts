import { AccountService } from 'src/app/account/account.service';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class UserResolver implements Resolve<any>{

  constructor(private accountService: AccountService) {

  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    // In my case i am using custom service for getting rest calls but you can use simple http.get()...
    return this.accountService.loadCurrentUser(localStorage.getItem('token'));
  }
}
