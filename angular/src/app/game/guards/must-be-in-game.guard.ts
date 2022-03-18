import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { of } from 'rxjs';
import { Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { GameService } from '../servicess/game.service';

@Injectable({
  providedIn: 'root',
})
export class MustBeInGameGuard implements CanActivate {
  constructor(private gameService: GameService, private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    let val = this.gameService.isInGame$.pipe(
      switchMap((x) => {
        if (x) return of(x);
        return this.router.navigate(['game']);
      })
    );
    return val;
  }
}
