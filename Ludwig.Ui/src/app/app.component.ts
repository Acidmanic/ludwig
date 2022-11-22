import {Component, OnDestroy, OnInit} from '@angular/core';
import {PriorityModel} from "./models/priority-model";
import {LoginManagerService} from "./services/login-manager/login-manager.service";

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


  constructor(public svcLogin:LoginManagerService) {
  }


  ngOnInit(): void {

  }

  ngOnDestroy(): void {

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
