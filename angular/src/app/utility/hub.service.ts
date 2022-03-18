import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthService } from '../auth/services/auth.service';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class HubService {
  private hub: signalR.HubConnection | undefined | null;
  constructor(private authService: AuthService) {
    this.userStatusChangeSubscription();
    this.connectionState$
      .pipe(filter((x) => x === signalR.HubConnectionState.Connected))
      .subscribe((x) => {
        this.on('placeOpened', () => this.placeOpened$.next());
        this.on('gameFinished', () => this.gameFinished$.next());
        this.on('playerJoined', () => {
          this.playerJoined$.next();
        });
      });
  }
  private userStatusChangeSubscription() {
    this.authService.userLoggedInStatus$.subscribe((x) => {
      if (
        !x &&
        this.hub &&
        this.hub.state === signalR.HubConnectionState.Connected
      ) {
        this.invoke('LeaveGame', () => {});
        return;
      }
      if (!this.hub || this.hub.state != signalR.HubConnectionState.Connected) {
        this.hub = this.getHubConnection();
        this.startConnection(this.hub);
      }
    });
  }

  private getHubConnection(): signalR.HubConnection {
    const route = `${
      environment.apis().hub
    }?access_token=${this.authService.accessToken()}`;
    console.log(route);

    return new signalR.HubConnectionBuilder()
      .withUrl(route, {
        logger: signalR.LogLevel.Critical,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect([
        1000,
        2000,
        3000,
        4000,
        8000,
        10000,
        10000,
        16000,
        30000,
        45000,
      ])
      .build();
  }

  private startConnection(hub: signalR.HubConnection) {
    hub.start().then(() => this.connectionState$.next(hub.state));
    hub.onreconnected(() => {
      this.connectionState$.next(hub.state);
      console.log('Successgully reconected');
    });
  }

  private on(method: functionCalls, callback: (...args: any) => void) {
    this.hub?.on(method, (args: any) => callback(args));
  }

  invoke(mehtod: serverCalls, callBack: (...args: any) => void, ...args: any) {
    this.hub?.invoke(mehtod, ...args).then((x: any) => callBack(x));
  }

  placeOpened$ = new Subject<void>();
  gameFinished$ = new Subject<void>();
  playerJoined$ = new Subject<void>();
  connectionState$ = new BehaviorSubject<signalR.HubConnectionState>(
    signalR.HubConnectionState.Disconnected
  );
}

type functionCalls = 'placeOpened' | 'gameFinished' | 'playerJoined';
type serverCalls =
  | 'CheckForOpenPlace'
  | 'GetInGame'
  | 'GetUserGames'
  | 'LeaveGame'
  | 'GetCurrentGame'
  | 'GetAttendingsCount'
  | 'SubmitAnswer';
