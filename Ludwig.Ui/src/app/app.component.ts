import {Component, OnDestroy, OnInit} from '@angular/core';
import {PriorityModel} from "./models/priority-model";
import {LoginManagerService} from "./services/login-manager/login-manager.service";
import {ExportInfoModel} from "./models/export-info-model";
import {ExportService} from "./services/export/export.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit,OnDestroy{

  title = 'Ludwig.Ui';


  exportInfos:ExportInfoModel[]=[];


  constructor(public svcLogin:LoginManagerService,
              private svcExports:ExportService) {
  }


  ngOnInit(): void {
    console.log("Main Component Initialized");

    this.svcExports.getAvailableExports().subscribe({
      next: info => {
        this.exportInfos=info;
        console.log('got exporters',this.exportInfos);
      },
      error:err => {
        console.log(err);
      },
      complete: () => {}
    });

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
