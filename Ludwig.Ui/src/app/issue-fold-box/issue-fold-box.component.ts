import {Component, Input, OnInit} from '@angular/core';
import {UserStoryModel} from "../models/user-story.model";

@Component({
  selector: 'issue-fold-box',
  templateUrl: './issue-fold-box.component.html',
  styleUrls: ['./issue-fold-box.component.css']
})
export class IssueFoldBoxComponent implements OnInit {

  constructor() { }

  @Input('story') story:UserStoryModel=new UserStoryModel();

  folded:boolean=true;

  ngOnInit(): void {
  }

  toggle(){
    this.folded = !this.folded;
  }

  countIssues(story:UserStoryModel):number{

    if(story){
      if(story.issues){
        return story.issues.length;
      }
    }
    return 0;
  }
}
