import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingTextComponent } from './loading-text/loading-text.component';



@NgModule({
  declarations: [LoadingTextComponent],
  imports: [
    CommonModule
  ],
  exports:[
    LoadingTextComponent
  ]
})
export class UtilityModule { }
