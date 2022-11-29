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

  private administratorUser:IssueManagerUserModel={
    userReferenceLink:'',
    displayName:'ludwig van beethoven',
    name:'Administrator',
    avatarUrl:'assets/images/default-profile.png',
    active:true,
    emailAddress:''
  };


  public login(model:object,methodName:string):Observable<boolean>{

    let handler = new Subject<boolean>();

    this.scvAuth.login(model,methodName).subscribe({

      next: token => {
        if (token.isAdministrator){
          this.isLoggedIn = true;
          this.me = {...this.administratorUser};
          this.token ={...token};
          this.saveLogin();
          handler.next(true);
          handler.complete();
        }else{
          this.svcIssueManager.getMeBeforeLoggedIn('token ' + token.token)
            .subscribe({
              next: me => {
                this.me = me;
                this.token = {...token};
                this.isLoggedIn = true;
                this.saveLogin();
                handler.next(true);
              },
              error:err=>handler.error(err),
              complete:()=>handler.complete()
            });
        }
      },
      error:err=>handler.error(err)
    });
    return handler;
  }

  private saveLogin(){

    this.svcStorage.storeData('LoginManagerService.me',this.me);
    this.svcStorage.storeData('LoginManagerService.isLoggedIn',this.isLoggedIn);
    this.svcStorage.storeData('LoginManagerService.token',this.token);

  }

  private loadLogin(){
    this.me=this.svcStorage.acquireData<IssueManagerUserModel>('LoginManagerService.me');
    this.token=this.svcStorage.acquireData<TokenModel>('LoginManagerService.token');
    this.isLoggedIn=this.svcStorage.acquireData<boolean>('LoginManagerService.isLoggedIn');

  }

  private clearLogin(){
    this.svcStorage.removeData('LoginManagerService.me');
    this.svcStorage.removeData('LoginManagerService.token');
    this.svcStorage.removeData('LoginManagerService.isLoggedIn');
  }


  public logOut():Observable<undefined>{

    let handler = new Subject<undefined>();

    if(this.isLoggedIn){
      this.scvAuth.logOut(this.token.token).subscribe({
        next: () => handler.next(undefined),
        error: err=>handler.error(err),
        complete: () => handler.complete()
      });
      this.isLoggedIn=false;
      this.me=new IssueManagerUserModel();
      this.clearLogin()
    }

    return handler;
  }
}
