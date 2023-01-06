import {Component, OnDestroy, OnInit} from '@angular/core';
import {PriorityModel} from "./models/priority-model";
import {LoginManagerService} from "./services/login-manager/login-manager.service";
import {ExportInfoModel} from "./models/export-info-model";
import {ExportService} from "./services/export/export.service";
import {NavigationStart, Router} from "@angular/router";
import {ProjectsService} from "./services/projects/projects.service";
import {ProjectModel} from "./models/project-model";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit,OnDestroy{

  title = 'Ludwig.Ui';
  projects:ProjectModel[]=[];

  exportInfos:ExportInfoModel[]=[];

  constructor(public svcLogin:LoginManagerService,
              private svcProjects:ProjectsService,
              private svcExports:ExportService,
              private router:Router) {

    this.router.events.subscribe({
      next: ev => {
        if (ev instanceof NavigationStart) {

          console.log('Route change detected');

          this.updateExportersMenu();
        }
      }
    });


  }


  ngOnInit(): void {
    console.log("Main Component Initialized");

    this.svcProjects.getAllProjects().subscribe({
      next: prj => this.projects = prj,
      error:e =>  console.log('error getting projects: ',e)
    });

  }

  private updateExportersMenu(){
    if(this.svcLogin.isLoggedIn){
      this.svcExports.getAvailableExports().subscribe({
        next: info => {
          this.exportInfos=info;
        },
        error:err => {
          console.log(err);
        },
        complete: () => {}
      });
    }
  }

  ngOnDestroy(): void {

  }

}
