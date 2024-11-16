import { Injectable } from '@angular/core';
import { NgxSpinnerService } from "ngx-spinner"

@Injectable({
  providedIn: 'root'
})
export class BusyService {

  busyReuqestCount = 0;
  constructor(private spinner: NgxSpinnerService) { }

  busy() {
    this.busyReuqestCount++;
    this.spinner.show();
  }

  idle() {
    this.busyReuqestCount--;
    if (this.busyReuqestCount <= 0) {
      this.busyReuqestCount = 0;
      this.spinner.hide();
    }
  }
}
