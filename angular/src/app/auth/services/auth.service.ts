import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterModel } from '@auth/model/registerModel';
import { ITokenHolder } from '@auth/model/token-holder';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { LoginModel } from '../model/login-model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private authKey = 'auth-token';
  private _currentToken: ITokenHolder | undefined;

  private get currentToken(): ITokenHolder | undefined {
    if (!this._currentToken) {
      this.retrieveFromStoage();
    }
    return this._currentToken;
  }

  private retrieveFromStoage() {
    const str = sessionStorage.getItem(this.authKey);
    if (str) {
      const data = JSON.parse(str);
      this._currentToken = {
        expiresAt: new Date(data.expiresAt),
        token: data.token,
      };
    }
  }

  private set currentToken(val: ITokenHolder | undefined) {
    this._currentToken = val;
    if (val) sessionStorage.setItem(this.authKey, JSON.stringify(val));
  }

  constructor(private httpClient: HttpClient) {}

  userLoggedInStatus$ = new BehaviorSubject<boolean>(this.isLoggedIn());

  init() {
    this.userLoggedInStatus$.next(this.isLoggedIn());
  }

  logout() {
    sessionStorage.removeItem(this.authKey);
    this.userLoggedInStatus$.next(false);
    return true;
  }

  isLoggedIn() {
    let time = this.currentToken?.expiresAt?.getTime();
    const r = time ? time > new Date().getTime() : false;
    if (!r && time) this.logout();
    return r;
  }

  bearerToken() {
    return `Bearer ${this.currentToken?.token}`;
  }

  accessToken() {
    return this.currentToken?.token ?? '';
  }

  register(model: RegisterModel) {
    if (!model) throw new Error('The register model is missing');
    return this.httpClient.post(environment.apis().register, model);
  }

  login(model: LoginModel) {
    if (!model) throw new Error('The login model is missing');
    return this.httpClient
      .post<ITokenHolder>(environment.apis().login, model)
      .pipe(
        tap((x: any) => {
          this.currentToken = {
            expiresAt: new Date(x.expiresAt),
            token: x.token,
          };
          this.userLoggedInStatus$.next(this.isLoggedIn());
        })
      );
  }
}
