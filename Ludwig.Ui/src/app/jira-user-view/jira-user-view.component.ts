import {Component, Input, OnInit} from '@angular/core';
import {JiraUserModel} from "../models/jira-user-model";

@Component({
  selector: 'jira-user-view',
  templateUrl: './jira-user-view.component.html',
  styleUrls: ['./jira-user-view.component.css']
})
export class JiraUserViewComponent implements OnInit {


  @Input('user') user:JiraUserModel=new JiraUserModel();
  @Input('size') size:string ='48';


  constructor() { }

  ngOnInit(): void {
  }


  profilePicture():string {
    if(this.user){
      switch (this.size){
        case '48': return this.user.avatarUrls["48x48"];
        case '32': return this.user.avatarUrls["32x32"];
        case '24': return this.user.avatarUrls["24x24"];
        case '16': return this.user.avatarUrls["16x16"];
      };
    }
    return '';
  }

  css():string {

    var css ='';

    if(this.user.active){
      css+='jirauser-active ';
    }

    return css;
  }

}
