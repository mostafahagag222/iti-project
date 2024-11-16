import { ToastrService } from 'ngx-toastr';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { Injectable } from '@angular/core';
import { delay, finalize, catchError } from 'rxjs/operators';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private busyService: BusyService, private router: Router, private toastr: ToastrService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                // console.log("i'm here 404")
                if (error) {
                    if (error.status === 400) {
                        console.log("i'm here 400")
                        if (error.error.errors) {
                            throw error.error;
                        } else {
                            this.toastr.error(error.error.message, error.error.statusCode);
                        }
                    }
                    if (error.status === 401) {
                        console.log("i'm here 401")

                        this.toastr.error(error.error.message, error.error.statusCode);
                    }
                    if (error.status === 404) {
                        this.toastr.error(error.error.message, error.error.statusCode);
                    }
                    if (error.status === 500) {
                        const navigationExtras: NavigationExtras = { state: { error: error.error } }
                        this.router.navigateByUrl('/server-error', navigationExtras);
                    }
                }

                return throwError(error);
            })
        )
    }
}