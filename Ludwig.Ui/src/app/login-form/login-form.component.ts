import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {LoginMethodModel} from "../models/login-method-model";
import {LoginFieldModel} from "../models/login-field-model";

interface LooseObject {
  [key: string]: any
}

@Component({
  selector: 'login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {

  @Input('method') method:LoginMethodModel=new LoginMethodModel();
  @Output('login') login:EventEmitter<object>= new EventEmitter<object>();



  loginModel:LooseObject={};

  constructor() {

    this.method.description="No Authentication Methods available.";
    this.method.name="";

  }

  ngOnInit(): void {

  }

  setValue(name:string,value:string){

    this.loginModel[name]=value;
  }

  getValue(name:string):string{
    if(this.loginModel[name]){
      return this.loginModel[name];
    }else{
      return "";
    }
  }

  inputType(field:LoginFieldModel):string{
    if(field.uiProtectedValue){
      return "password";
    }
    return "text";
  }

  loginClick(){
    console.log('login model:',this.loginModel);
  }

}
