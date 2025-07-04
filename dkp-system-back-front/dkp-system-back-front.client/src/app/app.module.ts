import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,     // Браузер
    HttpClientModule,  // Работа с HTTP
    FormsModule        // Двусторонняя привязка (ngModel)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
