import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import { ShoeBoxComponent } from './pages/shoe-box/shoe-box.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from "@angular/forms";
import { IssueItemComponent } from './issue-item/issue-item.component';
import { IssueFoldBoxComponent } from './issue-fold-box/issue-fold-box.component';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { WaitingLogoComponent } from './waiting-logo/waiting-logo.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { DisablerComponent } from './disabler/disabler.component';
import { MessageBoxComponent } from './message-box/message-box.component';
import { EditableTextBoxComponent } from './editable-text-box/editable-text-box.component';
import { EditableUserStoryComponent } from './editable-user-story/editable-user-story.component';
import { CollapsibleComponent } from './collapsible/collapsible.component';
import { JiraUserViewComponent } from './jira-user-view/jira-user-view.component';
import { EditablePriorityComponent } from './editable-priority/editable-priority.component';

@NgModule({
  declarations: [
    AppComponent,
    ShoeBoxComponent,
    IssueItemComponent,
    IssueFoldBoxComponent,
    WaitingLogoComponent,
    NotFoundComponent,
    DisablerComponent,
    MessageBoxComponent,
    EditableTextBoxComponent,
    EditableUserStoryComponent,
    CollapsibleComponent,
    JiraUserViewComponent,
    EditablePriorityComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    FormsModule,
    BrowserAnimationsModule,
  ],
  providers: [
    // {
    //   provide:HTTP_INTERCEPTORS,
    //   useClass:CookieInterceptor,
    //   multi:true
    // }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
