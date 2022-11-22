import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {LoginMethodModel} from "../models/login-method-model";
import {LoginFieldModel} from "../models/login-field-model";

interface LooseObject {
  [key: string]: string
}

@Component({
  selector: 'login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {

  @Input('method') method:LoginMethodModel=new LoginMethodModel();
  @Output('login') login:EventEmitter<object>= new EventEmitter<object>();
  @Input('login-error') loginError:boolean=false;


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


  disabledClass():string {

    if(!(this.method) || !(this.method.fields) || this.method.fields.length==0){
      return 'disabled';
    }

    for(let field of this.method.fields){

      let name = field.name;

      if(!(this.loginModel[name]) || this.loginModel[name].length<1)
        return 'disabled';

    }

    return '';
  }


  filterModel():object{

    let model: LooseObject = {};

    for(let field of this.method.fields){
      var name = field.name;
      var value = '';
      if(this.loginModel[name]){
        value = this.loginModel[name];
      }
      name = name.charAt(0).toLowerCase() + name.substring(1,name.length);

      model[name]=value;
    }
    return model;
  }


  loginClick(){

    let model = this.filterModel();

    this.login.emit(model);
  }

}
