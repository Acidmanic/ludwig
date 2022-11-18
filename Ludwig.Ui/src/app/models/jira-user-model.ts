import {ImageSrcMap} from "./image-src-map";


export class JiraUserModel{

  self:string="";
  key: string="";
  name:string="";
  emailAddress:string="";
  avatarUrls:ImageSrcMap=new ImageSrcMap();
  displayName:string="";
  active:boolean=false;
  timeZone:string="Asia/Tehran";
  locale:string= "en_US";
}
