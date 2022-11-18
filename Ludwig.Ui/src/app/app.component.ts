import {Component, OnInit} from '@angular/core';
import {CookieService} from "ngx-cookie-service/dist-lib";
import {ResultOf} from "./models/result-of";
import {JiraUserModel} from "./models/jira-user-model";
import {JiraService} from "./services/jira/jira.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{


  title = 'Ludwig.Ui';

  loggedInUser:JiraUserModel= new JiraUserModel();
  isLoggedIn:boolean=false;
  constructor(private svcJira:JiraService) {
  }


  ngOnInit(): void {
    this.svcJira.loggedIn().subscribe({
      next: loggedIn => {
        if(loggedIn.success){
          if(loggedIn.value){
            this.loggedInUser = loggedIn.value;
            this.isLoggedIn=true;
            console.log(this.loggedInUser);
          }
        }
      },
      error: e => {},
      complete: () => {}
    });
  }

}
