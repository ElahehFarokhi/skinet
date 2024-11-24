import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snckbar = inject(SnackbarService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 400) {
        if (err.error.errors) {
          const modelStateErrors = [];
          for (const key in err.error.errors) {
            if (err.error.errors[key]) {
              modelStateErrors.push(err.error.errors[key]);
            }
          }
          throw modelStateErrors.flat();
        } else {
          snckbar.error(err.error.title || err.error);
        }
      }
      if (err.status === 401) {
        snckbar.error(err.error.title || err.error);
      }
      if (err.status === 403) {
        snckbar.error('Forbidden');
      }
      if (err.status === 404) {
        router.navigateByUrl('/not-found');
      }
      if (err.status === 500) {
        const navigationExtra: NavigationExtras = {
          state: { error: err.error },
        };
        console.log('err.error ===========>', err.error);
        router.navigateByUrl('/server-error', navigationExtra);
      }
      return throwError(() => err);
    })
  );
};
