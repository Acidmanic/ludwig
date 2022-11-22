import {Component, OnDestroy, OnInit} from '@angular/core';
import {IssueManagerUserModel} from "./models/issue-manager-user-model";
import {PriorityModel} from "./models/priority-model";
import {LoginManagerService} from "./services/login-manager/login-manager.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit,OnDestroy{

  title = 'Ludwig.Ui';


  priorities:PriorityModel[]=[
    {name:'Highest',value:0},
    {name:'High',value:1},
    {name:'Medium',value:2},
    {name:'Low',value:3},
    {name:'Lowest',value:4},
  ];

  //
  // private subscription:Subscription=new Subscription();

  constructor(public svcLogin:LoginManagerService) {
  }


  ngOnInit(): void {
    // this.subscription=LoginManagerService.loginUpdate.subscribe({
    //   next: l => {
    //     this.isLoggedIn=LoginManagerService.isLoggedIn;
    //     this.me=LoginManagerService.me;
    //     console.log('login updated:',LoginManagerService.me);
    //   }
    // });
  }

  ngOnDestroy(): void {
    // console.log("destroying app.component");
    // if(this.subscription){
    //   this.subscription.unsubscribe();
    // }
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
