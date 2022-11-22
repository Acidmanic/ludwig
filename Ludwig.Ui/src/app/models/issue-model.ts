import {IssueTypeModel} from "./issue-type-model";
import {ProjectModel} from "./project-model";
import {JiraPriorityModel} from "./jira-priority-model";
import {IssueManagerUserModel} from "./issue-manager-user-model";


export class IssueModel{

  id:number=0;
  self:string="";
  key:string="";
  issueType:IssueTypeModel=new IssueTypeModel();
  summary:string="";
  description:string="";
  project:ProjectModel=new ProjectModel();
  priority:JiraPriorityModel=new JiraPriorityModel();
  assignee:IssueManagerUserModel=new IssueManagerUserModel();
  userStory:string="";
}
