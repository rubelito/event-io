import { isPlatformBrowser } from '@angular/common';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ApiErrorService } from './api-error.service';
import { clearStoredSession } from './auth-session';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    @Inject(PLATFORM_ID) private platformId: Object,
    private errors: ApiErrorService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const base = new URL(this.baseUrl);
    const url = new URL(request.url, base);
    const apiRoot = base.pathname.replace(/\/$/, '');
    const path = url.pathname.slice(apiRoot.length);
    if (url.origin !== base.origin || !url.pathname.startsWith(apiRoot + '/') || !/^\/(user|schedule)(\/|$)/i.test(path)) {
      return next.handle(request);
    }

    const isPublic = /^\/user\/(Login|Register|IsUserExist|IsEmailExist)\/?$/i.test(path);
    const browser = isPlatformBrowser(this.platformId);
    const token = browser && !isPublic ? localStorage.getItem('accessToken') : null;
    const authenticated = token && !request.headers.has('Authorization')
      ? request.clone({ setHeaders: { Authorization: token } })
      : request;

    return next.handle(authenticated).pipe(catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse && browser) {
        if (error.status === 401 && !isPublic && token && localStorage.getItem('accessToken') === token) {
          clearStoredSession();
          this.dialog.closeAll();
          void this.router.navigateByUrl('/');
        }
        this.errors.notify(error);
      }
      // Preserve status/body for callers; a failed request must never become success.
      return throwError(() => error);
    }));
  }
}
