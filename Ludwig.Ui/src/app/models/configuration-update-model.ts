import {ConfigurationItemModel} from "./configuration-item-model";
import {MessageModel} from "./message-model";


export class ConfigurationUpdateModel{

  success:boolean=false;
  items:ConfigurationItemModel[]=[];
  message:MessageModel=new MessageModel();
}
