import {Component, Input, OnInit} from '@angular/core';
import {IssueManagerUserModel} from "../models/issue-manager-user-model";
import {LoginManagerService} from "../services/login-manager/login-manager.service";

@Component({
  selector: 'login-button',
  templateUrl: './login-button.component.html',
  styleUrls: ['./login-button.component.css']
})
export class LoginButtonComponent implements OnInit {

  @Input('user') user:IssueManagerUserModel=new IssueManagerUserModel();


  constructor(private svcLogin:LoginManagerService) { }

  ngOnInit(): void {
  }


  logout(){

  }
}
