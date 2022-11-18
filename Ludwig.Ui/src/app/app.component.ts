import {Component, OnInit} from '@angular/core';
import {CookieService} from "ngx-cookie-service/dist-lib";
import {ResultOf} from "./models/result-of";
import {JiraUserModel} from "./models/jira-user-model";
import {JiraService} from "./services/jira/jira.service";
import {PriorityModel} from "./models/priority-model";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{


  title = 'Ludwig.Ui';

  loggedInUser:JiraUserModel= new JiraUserModel();
  isLoggedIn:boolean=false;

  priorities:PriorityModel[]=[
    {name:'Highest',value:0},
    {name:'High',value:1},
    {name:'Medium',value:2},
    {name:'Low',value:3},
    {name:'Lowest',value:4},
  ];

  constructor(private svcJira:JiraService) {
  }


  ngOnInit(): void {
    this.svcJira.loggedIn().subscribe({
      next: loggedIn => {
        if(loggedIn.success){
          if(loggedIn.value){
            this.loggedInUser = loggedIn.value;
            this.isLoggedIn=true;
          }
        }
      },
      error: e => {},
      complete: () => {}
    });
  }

}
