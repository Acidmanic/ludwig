import {Component, ElementRef, Input, OnInit} from '@angular/core';
import {IssueModel} from "../models/issue-model";

@Component({
  selector: 'issue-item',
  templateUrl: './issue-item.component.html',
  styleUrls: ['./issue-item.component.css']
})
export class IssueItemComponent implements OnInit {

  constructor() { }

  @Input('issue') issue:IssueModel=new IssueModel();
  @Input('outerElement') outerElement?:ElementRef;

  ngOnInit(): void {
  }

  caption(issue:IssueModel):string{
    var caption = issue.title;
    let width = this.textLength();
    if(caption && caption.length>width){
      caption = caption.substring(0,(width-3)) + '...';
    }
    return caption;
  }


  textLength(){
    if(this.outerElement){
      let w = this.outerElement.nativeElement.offsetWidth/10;
      if(w >3){
        return w;
      }
    }
    return 20;
  }

}
