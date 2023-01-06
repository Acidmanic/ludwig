import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {read} from "@popperjs/core";

@Component({
  selector: 'project-story-map',
  templateUrl: './project-story-map.component.html',
  styleUrls: ['./project-story-map.component.css']
})
export class ProjectStoryMapComponent implements OnInit {

  public projectId:number=0;

  constructor(private svcActivate:ActivatedRoute) { }



  ngOnInit(): void {

    var readId = this.svcActivate.snapshot.paramMap.get('id');

    if(readId){
      this.projectId=+readId;
    }


  }

}
