import { Component, OnInit } from '@angular/core';
import {LoginMethodModel} from "../../models/login-method-model";
import {AuthenticationService} from "../../services/authentication-service/authentication-service";
import {WaiterService} from "../../services/waiter.service";
import {LoginManagerService} from "../../services/login-manager/login-manager.service";
import {Router} from "@angular/router";
import {StructuredLocalStorageService} from "../../services/structured-local-storage.service";
import {ResultOf} from "../../models/result-of";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginMethods:LoginMethodModel[]=new Array<LoginMethodModel>();
  selectedMethod:LoginMethodModel= new LoginMethodModel();
  loginError:boolean=false;

  constructor(private svcAuth:AuthenticationService,
              private svcWait:WaiterService,
              private svcLogin:LoginManagerService,
              private router:Router,
              private svcStorage:StructuredLocalStorageService) {

    this.selectedMethod.description="No Authentication Methods available.";
    this.selectedMethod.name="";
  }

  ngOnInit(): void {

    this.svcWait.start();

    this.svcAuth.getLoginMethods().subscribe({
      next: methods => {
        this.loginMethods = methods;
        if(methods.length && methods.length>0){

          let methodFound = this.findSelectedMethod(localStorage.getItem('selected-login-method'));

          if(methodFound.success){
            this.selectedMethod = methodFound.value!;
          }else{
            this.selectedMethod = methods[0];
            localStorage.setItem('selected-login-method',this.selectedMethod.name);
          }

        }
      },
      error: err => {
        this.svcWait.stop();
        console.log('error',err);
      },
      complete: () => this.svcWait.stop()
    });

  }


  private findSelectedMethod(name:string | null):ResultOf<LoginMethodModel>{

    if(name){
      for(let method of this.loginMethods){
        if(method.name==name){
          return {value:method,success:true};
        }
      }
    }
    return {value:undefined,success:false};
  }

  activeClass(method:LoginMethodModel):string{
    if(method){
      if(this.selectedMethod){
        if(method.name === this.selectedMethod.name){
          return "active bg-info"
        }
      }
    }
    return "";
  }

  onMethodClick(method:LoginMethodModel){
    this.selectedMethod=method;
    localStorage.setItem('selected-login-method',this.selectedMethod.name);
    this.loginError=false;
  }
  headerCaption():string{
    if(this.loginMethods.length==1){
      return 'Please Login';
    }
    return 'Please Choose Your Authentication Method';
  }

  loginClicked(model:any){

    this.loginError = false;

    this.svcWait.start();

    this.svcLogin.login(model,this.selectedMethod.name).subscribe({
      next: token => {
        this.svcWait.stop();
        if(this.svcLogin.token.isIssueManager){
          this.router.navigate(['shoe-box']);
        }else{
          this.router.navigate(['configure']);
        }

      },
      error: err => {
        this.svcWait.stop();
        this.loginError = true;
      },
      complete: () => this.svcWait.stop()
    });

  }


}
