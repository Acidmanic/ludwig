import {Component, Input, OnInit} from '@angular/core';
import {LoginManagerService} from "../services/login-manager/login-manager.service";

@Component({
  selector: 'access-denied',
  templateUrl: './access-denied.component.html',
  styleUrls: ['./access-denied.component.css']
})
export class AccessDeniedComponent implements OnInit {

  userType:string='User Type';
  @Input('page-title') pageTitle:string='Page Title';
  @Input('view-to') viewTo:string='administrator';

  constructor(private svcLogin:LoginManagerService) { }

  ngOnInit(): void {
    this.userType = this.getUserRole();
  }


  getUserRole(){
    if(this.svcLogin.isLoggedIn){
        if(this.svcLogin.token.isIssueManager){
          return 'Issue Manager';
        }
      if(this.svcLogin.token.isAdministrator){
        return 'Administrator';
      }
    }
    return "not-logged-in";
  }

  visible():boolean{

    if(this.svcLogin.isLoggedIn){

      if(this.svcLogin.token.isIssueManager && this.viewTo=='issue-manager'){
        return true;
      }
      if(this.svcLogin.token.isAdministrator && this.viewTo=='administrator'){
        return true;
      }
      return false;
    }
    return true;
  }
}
