import {LoginFieldModel} from "./login-field-model";
import {UiLinkModel} from "./ui-link-model";

export class LoginMethodModel{

  fields:LoginFieldModel[]=[];
  description:string='';
  link?:UiLinkModel;
  name:string='';

}
