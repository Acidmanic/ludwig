import { Component, OnInit } from '@angular/core';
import {LoginMethodModel} from "../../models/login-method-model";
import {AuthenticationService} from "../../services/authentication-service/authentication-service";
import {WaiterService} from "../../services/waiter.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginMethods:LoginMethodModel[]=new Array<LoginMethodModel>();
  selectedMethod:LoginMethodModel= new LoginMethodModel();

  constructor(private svcAuth:AuthenticationService,
              private svcWait:WaiterService) {

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
            name:'One Time Password',
            uiProtectedValue:true
          }],
          name:'OTP',
          description:'Please Enter the code you have received.'
        });
        console.log('methods',methods);
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

}
