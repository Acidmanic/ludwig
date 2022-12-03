import {LoginFieldModel} from "./login-field-model";
import {UiLinkModel} from "./ui-link-model";
import {QueryInputModel} from "./query-input-model";

export class LoginMethodModel{

  fields:LoginFieldModel[]=[];
  description:string='';
  queries:QueryInputModel[]=[];
  link?:UiLinkModel;
  name:string='';
  iconUrl?:string=undefined;
}
