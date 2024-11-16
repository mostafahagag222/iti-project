import { AccountService } from './../account.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  returnUrl: string;

  constructor(private accountService: AccountService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    // if (this.accountService.getCurrentUserValue() !== null) {

    //   this.router.navigateByUrl('/');
    // }
    // this.returnUrl = '/shop';
    this.createLoginForm();
  }

  createLoginForm() {

    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', Validators.required)
    });
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe(() => {

      console.log('user logged in')
      this.router.navigateByUrl(this.returnUrl);
    },
      error => {
        console.log(error)
      })
  }

}
