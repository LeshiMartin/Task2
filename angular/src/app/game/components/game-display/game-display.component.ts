import { Component, HostListener, OnInit } from '@angular/core';
import { GameService } from '../../servicess/game.service';
import { GameModel } from '../../models/game-model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-game-display',
  templateUrl: './game-display.component.html',
  styleUrls: ['./game-display.component.scss'],
})
export class GameDisplayComponent implements OnInit {
  constructor(private gameService: GameService) {}
  games$ = this.gameService.games$;
  currentGame$ = this.gameService.currentGame$;
  title$ = this.gameService.title$;
  destroyer$ = new Subject<void>();
  ngOnInit(): void {
    this.gameService.getGames();
    this.gameService.getAttendingsTitle();
    this.gameService.playerJoined$
      .pipe(takeUntil(this.destroyer$))
      .subscribe(() => {
        this.gameService.getAttendingsTitle();
      });
    this.gameService.placeOpened$
      .pipe(takeUntil(this.destroyer$))
      .subscribe(() => {
        this.gameService.getAttendingsTitle();
      });
  }
  ngOnDestroy(): void {
    this.gameService.leaveGame();
    this.destroyer$.next();
    this.destroyer$.complete();
  }

  @HostListener('window:beforeunload', ['$event'])
  beforeunloadHandler(event: any) {
    this.gameService.leaveGame();
  }

  submitValue(val: string, id: number) {
    this.gameService.submitAnswer(val, id);
  }

  trackById(index: number, item: GameModel) {
    return item.id;
  }
}
