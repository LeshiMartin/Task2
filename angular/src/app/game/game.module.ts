import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameDisplayComponent } from './components/game-display/game-display.component';
import { GameStartScreenComponent } from './components/game-start-screen/game-start-screen.component';
import { GameRoutingModule } from './game-routing.module';
import { MatCardModule } from '@angular/material/card';
import { StatusPipe } from './components/game-display/status.pipe';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [GameDisplayComponent, GameStartScreenComponent, StatusPipe],
  imports: [CommonModule, GameRoutingModule, MatCardModule, MatButtonModule],
  exports: [GameRoutingModule],
})
export class GameModule {}
