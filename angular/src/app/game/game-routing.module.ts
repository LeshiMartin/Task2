import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { GameStartScreenComponent } from './components/game-start-screen/game-start-screen.component';
import { GameDisplayComponent } from './components/game-display/game-display.component';
import { MustBeInGameGuard } from './guards/must-be-in-game.guard';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: 'home',
    component: GameStartScreenComponent,
  },
  {
    path: 'game-display',
    canActivate: [MustBeInGameGuard],
    component: GameDisplayComponent,
  },
];

@NgModule({
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GameRoutingModule {}
