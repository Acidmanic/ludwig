import {IssueTypeModel} from "./issue-type-model";
import {ProjectModel} from "./project-model";
import {JiraPriorityModel} from "./jira-priority-model";
import {IssueManagerUserModel} from "./issue-manager-user-model";
import {PriorityModel} from "./priority-model";


export class IssueModel{

  issueReferenceLink:string="";
  issueType:IssueTypeModel=new IssueTypeModel();
  title:string="";
  description:string="";
  project:ProjectModel=new ProjectModel();
  priority:PriorityModel=new PriorityModel();
  assignees:IssueManagerUserModel[]=[];
  userStory:string="";
}
