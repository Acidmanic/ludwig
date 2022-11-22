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
      this.loadLogin();
  }


  public isLoggedIn:boolean=false;
  public token:TokenModel=new TokenModel();
  public me:IssueManagerUserModel=new IssueManagerUserModel();
  public loginUpdate:Subject<boolean>=new Subject<boolean>();


  public login(model:object,methodName:string):Observable<boolean>{

    let handler = new Subject<boolean>();

    this.scvAuth.login(model,methodName).subscribe({
      next: token => {

        console.log('authorized, going to get logged user');

        this.svcIssueManager.getMeBeforeLoggedIn('token ' + token.token)
          .subscribe({
          next: me => {
            console.log('assigning to static varables');
            this.me = me;
            this.token = {...token};
            this.isLoggedIn = true;
            console.log('saving login.');
            this.saveLogin();
            console.log('calling subscribers');
            this.loginUpdate.next(true);
          },
            error:err=>handler.error(err),
            complete:()=>handler.complete()
        });
      },error:err=>handler.error(err),
      complete:()=>handler.complete()
    });

    return handler;
  }

  private saveLogin(){

    this.svcStorage.storeData('LoginManagerService.me',this.me);
    this.svcStorage.storeData('LoginManagerService.isLoggedIn',this.isLoggedIn);
    this.svcStorage.storeData('LoginManagerService.token',this.token);

  }

  public loadLogin(){
    this.me=this.svcStorage.acquireData<IssueManagerUserModel>('LoginManagerService.me');
    this.token=this.svcStorage.acquireData<TokenModel>('LoginManagerService.token');
    this.isLoggedIn=this.svcStorage.acquireData<boolean>('LoginManagerService.isLoggedIn');

    this.loginUpdate.next(this.isLoggedIn);
  }

  private clearLogin(){
    this.svcStorage.removeData('LoginManagerService.me');
    this.svcStorage.removeData('LoginManagerService.token');
    this.svcStorage.removeData('LoginManagerService.isLoggedIn');
  }

}
