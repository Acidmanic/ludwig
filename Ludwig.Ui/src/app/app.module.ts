import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule} from "@angular/common/http";
import { ShoeBoxComponent } from './pages/shoe-box/shoe-box.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from "@angular/forms";
import { IssueItemComponent } from './issue-item/issue-item.component';
import { IssueFoldBoxComponent } from './issue-fold-box/issue-fold-box.component';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { WaitingLogoComponent } from './waiting-logo/waiting-logo.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';

@NgModule({
  declarations: [
    AppComponent,
    ShoeBoxComponent,
    IssueItemComponent,
    IssueFoldBoxComponent,
    WaitingLogoComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    FormsModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
