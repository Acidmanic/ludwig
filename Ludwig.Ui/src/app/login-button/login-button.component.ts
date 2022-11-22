import {Component, Input, OnInit} from '@angular/core';
import {IssueManagerUserModel} from "../models/issue-manager-user-model";
import {LoginManagerService} from "../services/login-manager/login-manager.service";
import {WaiterService} from "../services/waiter.service";
import {Router} from "@angular/router";

@Component({
  selector: 'login-button',
  templateUrl: './login-button.component.html',
  styleUrls: ['./login-button.component.css']
})
export class LoginButtonComponent implements OnInit {

  @Input('user') user:IssueManagerUserModel=new IssueManagerUserModel();


  constructor(private svcLogin:LoginManagerService,
              private router:Router) { }

  ngOnInit(): void {
  }


  logout(){

    this.svcLogin.logOut().subscribe({
      next: () => {
        this.router.navigateByUrl('login');
      }
    });
  }
}
