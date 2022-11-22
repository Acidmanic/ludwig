import { Injectable } from '@angular/core';
import {AuthenticationService} from "../authentication-service/authentication-service";
import {Observable, Subject} from "rxjs";
import {TokenModel} from "../../models/token-model";
import {IssueManagerUserModel} from "../../models/issue-manager-user-model";
import {IssueManagerServiceService} from "../issue-manager/issue-manager-service.service";
import {StructuredLocalStorageService} from "../structured-local-storage.service";

@Injectable({
  providedIn: 'root'
})
export class LoginManagerService {

  constructor(private scvAuth:AuthenticationService,
              private svcIssueManager:IssueManagerServiceService,
              private svcStorage:StructuredLocalStorageService) {

    if(!LoginManagerService.isLoaded){
      this.loadLogin();
      LoginManagerService.isLoaded=true;
    }

  }


  public static isLoggedIn:boolean=false;
  public static token:TokenModel=new TokenModel();
  public static me:IssueManagerUserModel=new IssueManagerUserModel();
  private static isLoaded:boolean = false;

  public login(model:object,methodName:string):Observable<boolean>{

    let handler = new Subject<boolean>();

    this.scvAuth.login(model,methodName).subscribe({
      next: token => {
        this.svcIssueManager.getMeBeforeLoggedIn('token ' + token.token)
          .subscribe({
          next: me => {
            LoginManagerService.me = me;
            LoginManagerService.token = {...token};
            LoginManagerService.isLoggedIn = true;
            this.saveLogin();
          },
            error:handler.error,
            complete:handler.complete
        });
      },error:handler.error,
      complete:handler.complete
    });

    return handler;
  }

  private saveLogin(){

    this.svcStorage.storeData('LoginManagerService.me',LoginManagerService.me);
    this.svcStorage.storeData('LoginManagerService.isLoggedIn',LoginManagerService.isLoggedIn);
    this.svcStorage.storeData('LoginManagerService.token',LoginManagerService.token);

  }

  private loadLogin(){
    LoginManagerService.me=this.svcStorage.acquireData<IssueManagerUserModel>('LoginManagerService.me');
    LoginManagerService.token=this.svcStorage.acquireData<TokenModel>('LoginManagerService.token');
    LoginManagerService.isLoggedIn=this.svcStorage.acquireData<boolean>('LoginManagerService.isLoggedIn');
  }

  private clearLogin(){
    this.svcStorage.removeData('LoginManagerService.me');
    this.svcStorage.removeData('LoginManagerService.token');
    this.svcStorage.removeData('LoginManagerService.isLoggedIn');
  }

}
