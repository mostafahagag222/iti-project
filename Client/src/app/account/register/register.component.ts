import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    // if (this.accountService.getCurrentUserValue() !== null) {

    //   this.router.navigateByUrl('/');
    // }
    // console.log(this.accountService.getCurrentUserValue())
    this.createRegisterForm();
  }

  createRegisterForm() {

    this.registerForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required),
      gender: new FormControl('', Validators.required)
    });
  }

  changeCity(e: any) {
    this.gender?.setValue(e.target.value, {
      onlySelf: true,
    });
  }

  get gender() {
    return this.registerForm.get('gender');
  }

  onSubmit() {

    let { username, email, password, gender } = this.registerForm.value;
    let registerObj = {
      username,
      email,
      password,
      gender: parseInt(gender)
    }

    this.accountService.register(registerObj).subscribe((user) => {

      console.log('sign Up')

      console.log(this.registerForm.value)
      this.router.navigateByUrl('/');
    },
      error => {
        console.log(error)
      })
  }

}
