import {Component, Input, OnInit} from '@angular/core';
import {LoginManagerService} from "../services/login-manager/login-manager.service";
import {UserRoleModel} from "./user-role.model";
import {TokenModel} from "../models/token-model";

@Component({
  selector: 'access-denied',
  templateUrl: './access-denied.component.html',
  styleUrls: ['./access-denied.component.css']
})
export class AccessDeniedComponent implements OnInit {



  userType:string='User Type';
  @Input('page-title') pageTitle:string='Page Title';
  @Input('allowed-role-names') allowedRoleNames:string[]=[UserRoleModel.administrator.roleName];
  denyMessage:string='access-denied';
  constructor(private svcLogin:LoginManagerService) { }

  ngOnInit(): void {
    this.userType = this.getUserRole();
    this.denyMessage=this.getDenyMessage();
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

      let roles = this.getRoles(this.svcLogin.token);

      for(let role of roles){
        for(let al of this.allowedRoleNames){
          if(role.sameRole(al)){
            return false;
          }
        }
      }
    }
    return true;
  }

  private getRoles(token:TokenModel):UserRoleModel[]{

    let roles:UserRoleModel[] = [];

    if(token.isAdministrator){
      roles.push(UserRoleModel.administrator);
    }
    if(token.isIssueManager){
      roles.push(UserRoleModel.issueManager);
    }
    return roles;
  }

  private getDenyMessage():string{

    if(!this.allowedRoleNames || this.allowedRoleNames.length==0){
      return "apparently no one is allowed to see this page's content." +
        " This must be the UI developer's fault to add allowed roles for this page.";
    }

    var roles ='';

    var sep ='';

    for(let roleName of this.allowedRoleNames){
      roles += sep + roleName;
      sep = ' or ';
    }
    var s = (this.allowedRoleNames.length>1)?'s':'';

    var t = this.userType.toLowerCase().charAt(0);

    var n = (t=='a'||t=='i'||t=='o'||t=='e')?'n':'';

    return 'As a' + n + ' ' + this.userType+', you can not access this page.' +
      ' please login as a user with '+roles+' role'+s+' to' +
      ' access '+this.pageTitle+'.';
  }
}
