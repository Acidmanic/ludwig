import {Component, OnInit} from '@angular/core';
import {JiraUserModel} from "./models/jira-user-model";
import {JiraService} from "./services/jira/jira.service";
import {PriorityModel} from "./models/priority-model";
import {HttpClient} from "@angular/common/http";

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

  constructor(private svcJira:JiraService,
              private http:HttpClient) {
  }


  ngOnInit(): void {

  }

  //
  // usernameValue:string='Acidmanic';
  // passwordValue:string='sphere';
  //
  // authorize(){
  //
  //   let url = 'http://litbid.ir:8888/rest/api/2/myself'
  //   let usernamePassword = this.usernameValue+':'+this.passwordValue;
  //   let token = btoa(usernamePassword);
  //   let authorization = 'Basic ' + token;
  //
  //
  //   this.http.get(url,{headers:{authorization:authorization}}).subscribe({
  //     next: valu => console.log('Received as auth response',valu),
  //     error: err => console.log('Received auth Error',err),
  //     complete: () => {}
  //   });
  // }
  //
}
