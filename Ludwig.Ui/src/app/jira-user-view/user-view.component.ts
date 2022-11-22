import {Component, Input, OnInit} from '@angular/core';
import {IssueManagerUserModel} from "../models/issue-manager-user-model";
import {ImageSrcMap} from "../models/image-src-map";
import {MapIndex} from "../utilities/map-index";

@Component({
  selector: 'user-view',
  templateUrl: './user-view.component.html',
  styleUrls: ['./user-view.component.css']
})
export class UserViewComponent implements OnInit {


  @Input('user') user:IssueManagerUserModel=new IssueManagerUserModel();
  @Input('size') size:string ='48';
  @Input('active-user-class') activeUserClass:string='';
  @Input('link-profile') linkProfile:boolean=true;

  constructor() { }

  ngOnInit(): void {
  }

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
