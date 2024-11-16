import { AccountService } from './account/account.service';
import { Component, OnInit } from '@angular/core';
import { IUser } from './shared/models/user';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  currentUser$: Observable<IUser>;
  title = 'Furnitica';


  constructor(private accountService: AccountService, public router: Router) {


  }
  ngOnInit(): void {

    this.loadCurrentUser()
    this.loadWishList()
  }

  loadCurrentUser() {

    const token = localStorage.getItem('token');
    if (token) {
      this.accountService.loadCurrentUser(token).subscribe(() => {
      }, error => {
        console.log(error)
      })
    }
  }

  loadWishList() {

    const token = localStorage.getItem('token');
    if (token) {
      this.accountService.getWishList(token).subscribe(() => {
      }, error => {
        console.log(error)
      })
    }
  }
}
