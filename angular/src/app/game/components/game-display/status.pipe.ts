import { Pipe, PipeTransform } from '@angular/core';
import { GameModel } from '../../models/game-model';

@Pipe({
  name: 'status',
})
export class StatusPipe implements PipeTransform {
  transform(value: GameModel[] | null): number {
    return value?.filter((x) => x.answerStatus == 'OK')?.length ?? 0;
  }
}
