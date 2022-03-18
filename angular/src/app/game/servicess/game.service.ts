import { Injectable } from '@angular/core';
import { GameModel } from '@game/models/game-model';
import { BehaviorSubject, Subject } from 'rxjs';
import { HubService } from '../../utility/hub.service';
import { AuthService } from '../../auth/services/auth.service';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  constructor(
    private hubService: HubService,
    private authService: AuthService
  ) {
    this.hubService.gameFinished$.subscribe((x) => {
      this.currentGame$.next(null);
      this.getGames();
    });
    this.hubService.placeOpened$.subscribe((x) => {
      this.placeOpened$.next(true);
    });
    this.authService.userLoggedInStatus$
      .pipe(filter((x) => !!x))
      .subscribe((x) => {
        this.games$.next([]);
        this.currentGame$.next(null);
        this.isIngame = false;
        this.isInGame$.next(false);
      });
  }
  private isIngame = false;
  isInGame$ = new BehaviorSubject<boolean>(this.isIngame);
  placeOpened$ = new BehaviorSubject<boolean>(false);
  games$ = new BehaviorSubject<GameModel[]>([]);
  currentGame$ = new BehaviorSubject<GameModel | null>(null);
  hubState$ = this.hubService.connectionState$;
  playerJoined$ = this.hubService.playerJoined$;
  title$ = new Subject<string>();
  getInGame() {
    this.hubService.invoke('GetInGame', (x: boolean) => {
      this.isIngame = x;
      this.isInGame$.next(x);
      this.placeOpened$.next(false);
    });
  }

  checkForOpenSeat() {
    this.hubService.invoke('CheckForOpenPlace', (val) => {
      this.placeOpened$.next(val);
    });
  }

  leaveGame() {
    this.hubService.invoke('LeaveGame', () => {});
  }

  getAttendingsTitle() {
    this.hubService.invoke('GetAttendingsCount', (t) => this.title$.next(t));
  }

  getGames() {
    this.hubService.invoke('GetUserGames', (games: GameModel[]) => {
      this.games$.next(games);
      this.getCurrentGame();
    });
  }

  getCurrentGame() {
    this.hubService.invoke('GetCurrentGame', (game: GameModel) => {
      this.currentGame$.next(game);
    });
  }

  submitAnswer(answer: string, gameId: number) {
    this.hubService.invoke(
      'SubmitAnswer',
      (val) => {
        if (!val) this.getGames();
      },
      answer,
      gameId
    );
  }
}
