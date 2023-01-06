import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {read} from "@popperjs/core";
import {ProjectModel} from "../../models/project-model";
import {ProjectsService} from "../../services/projects/projects.service";
import {WaiterService} from "../../services/waiter.service";

@Component({
  selector: 'project-story-map',
  templateUrl: './project-story-map.component.html',
  styleUrls: ['./project-story-map.component.css']
})
export class ProjectStoryMapComponent implements OnInit {

  public projectId:number=0;
  public validProject:boolean=false;
  public project:ProjectModel=new ProjectModel();

  constructor(private svcActivate:ActivatedRoute,
              private svcProjects:ProjectsService,
              private svcWait:WaiterService) { }



  ngOnInit(): void {

    var readId = this.svcActivate.snapshot.paramMap.get('id');

    if(readId){
      this.projectId=+readId;

      this.svcWait.start();

      this.svcProjects.getById(this.projectId).subscribe({
        next: value => {
          this.project = value;
          this.validProject=true;
        },
        error: err => {
          this.svcWait.stop();
        },
        complete: () =>{
          this.svcWait.stop();
        }
      });

    }


  }

}
