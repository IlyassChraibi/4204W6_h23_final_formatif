import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // Ajoute le header d'authentification avec le token stocké dans le localStorage
    request = this.addAuthorizationHeader(request);

    return next.handle(request).pipe(
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          // Si la réponse est une HttpResponse, vérifie si elle contient un token d'authentification
          const authorizationHeader = event.headers.get('Authorization');
          if (authorizationHeader) {
            // Stocke le token d'authentification dans le localStorage
            localStorage.setItem(
              'token',
              authorizationHeader.replace('Bearer ', '')
            );
          }
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // Redirige vers la page de login si une erreur 401 est retournée
          this.router.navigateByUrl('/login');
        }
        return throwError(error);
      })
    );
  }

  private addAuthorizationHeader(request: HttpRequest<any>): HttpRequest<any> {
    const token = localStorage.getItem('token');
    if (token) {
      // Ajoute le header d'authentification avec le token stocké dans le localStorage
      return request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }
    return request;
  }
}
