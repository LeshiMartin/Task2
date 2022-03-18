import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { finalize, tap, retry, catchError } from 'rxjs/operators';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HttpEventType,
} from '@angular/common/http';
import { HttpErrorFactoryProvider } from './httpError/httpError-factory-provider';
import { ToasterService } from '@utility/toaster.service';
import { AuthService } from '@auth/services/auth.service';
@Injectable({
  providedIn: 'root',
})
export class HttpInterceptorService implements HttpInterceptor {
  constructor(
    private factoryProvider: HttpErrorFactoryProvider,
    private toasterService: ToasterService,
    private autService: AuthService
  ) {}
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const authReq = req.clone({
      headers: req.headers.set('Authorization', this.autService.bearerToken()),
    });
    return next.handle(authReq).pipe(
      finalize(() => {}),
      tap((ev: HttpEvent<any>) => {
        if (
          ev.type == HttpEventType.Response &&
          ev.body &&
          ev.body.errors &&
          ev.body.errors.lenght > 0
        ) {
          const message = ev.body.errors[0].extensions
            ? ev.body.errors[0].extensions.message
            : ev.body.errors[0].message;
        }
      }),
      retry(
        req.body && req.body['query'] && req.body['query'].includes('query')
          ? 1
          : 0
      ),
      catchError((exception: HttpErrorResponse) => {
        let handler = this.factoryProvider.getHandler(exception);
        this.displayMessage(handler.Handle(exception));
        return throwError(exception);
      })
    );
  }

  private displayMessage(msg: string): void {
    this.toasterService.error(msg);
  }
}
