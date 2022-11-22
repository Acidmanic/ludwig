import {Component, Input, OnInit} from '@angular/core';
import {IssueManagerUserModel} from "../models/issue-manager-user-model";
import {ImageSrcMap} from "../models/image-src-map";
import {MapIndex} from "../utilities/map-index";

@Component({
  selector: 'jira-user-view',
  templateUrl: './jira-user-view.component.html',
  styleUrls: ['./jira-user-view.component.css']
})
export class JiraUserViewComponent implements OnInit {


  @Input('user') user:IssueManagerUserModel=new IssueManagerUserModel();
  @Input('size') size:string ='48';
  @Input('active-user-class') activeUserClass:string='';

  //private avatarMapIndex:MapIndex=new MapIndex();

  constructor() { }

  ngOnInit(): void {
  }
  //
  // profilePicture():string {
  //
  //   this.avatarMapIndex = MapIndex.fromImageSrcMap(this.user.avatarUrls);
  //
  //   var src = '';
  //
  //   if(this.user && this.user.avatarUrls){
  //
  //     let size = parseInt(this.size);
  //
  //     src = this.avatarMapIndex.getSrcAnyway(size);
  //   }
  //
  //   return src;
  // }

  css():string {

    var css ='';

    if(!this.activeUserClass || this.activeUserClass.length==0){
      css+= ' auto-outline';
    }

    if(this.user.active){
      css+= ' '+this.activeUserClass;
    }
    return css;
  }

}
