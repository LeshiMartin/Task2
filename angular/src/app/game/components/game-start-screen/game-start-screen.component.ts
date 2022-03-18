import { Component, OnInit } from '@angular/core';
import { HubConnectionState } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';
import { GameService } from '../../servicess/game.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-game-start-screen',
  templateUrl: './game-start-screen.component.html',
  styleUrls: ['./game-start-screen.component.scss'],
})
export class GameStartScreenComponent implements OnInit {
  constructor(private gameService: GameService, private router: Router) {}
  private destroyer$ = new Subject<void>();
  placeIsOpened$ = this.gameService.placeOpened$;
  ngOnInit(): void {
    this.gameService.hubState$
      .pipe(
        filter((x) => x === HubConnectionState.Connected),
        takeUntil(this.destroyer$)
      )
      .subscribe((x) => {
        this.gameService.checkForOpenSeat();
      });
    this.gameService.isInGame$
      .pipe(filter((x) => !!x))
      .subscribe((x) => this.router.navigate(['../game', 'game-display']));
  }

  joinGame() {
    this.gameService.getInGame();
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.destroyer$.next();
    this.destroyer$.complete();
  }
}
