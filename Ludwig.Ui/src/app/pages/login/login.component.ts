import { Component, OnInit } from '@angular/core';
import {LoginMethodModel} from "../../models/login-method-model";
import {AuthenticationService} from "../../services/authentication-service/authentication-service";
import {WaiterService} from "../../services/waiter.service";
import {LoginManagerService} from "../../services/login-manager/login-manager.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginMethods:LoginMethodModel[]=new Array<LoginMethodModel>();
  selectedMethod:LoginMethodModel= new LoginMethodModel();

  constructor(private svcAuth:AuthenticationService,
              private svcWait:WaiterService,
              private svcLogin:LoginManagerService,
              private router:Router) {

    this.selectedMethod.description="No Authentication Methods available.";
    this.selectedMethod.name="";
  }

  ngOnInit(): void {

    this.svcWait.start();

    this.svcAuth.getLoginMethods().subscribe({
      next: methods => {
        this.loginMethods = methods;
        if(methods.length && methods.length>0){
          this.selectedMethod = methods[0];
        }
        this.loginMethods.push({
          fields:[{
            name:'otp',
            uiProtectedValue:true,
            description:'It will expire at 12:23am.',
            displayName:'One Time Password'
          }],
          name:'OTP Authentication',
          description:'Please Enter the code you have received.'
        });
      },
      error: err => {
        this.svcWait.stop();
        console.log('error',err);
      },
      complete: () => this.svcWait.stop()
    });

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

  headerCaption():string{
    if(this.loginMethods.length==1){
      return 'Please Login';
    }
    return 'Please Choose Your Authentication Method';
  }

  loginClicked(model:any){

    this.svcWait.start();

    this.svcLogin.login(model,this.selectedMethod.name).subscribe({
      next: token => {
        this.svcWait.stop();
        this.router.navigate(['/']);
      },
      error: err => {
        this.svcWait.stop();
      },
      complete: () => this.svcWait.stop()
    });

  }


}
