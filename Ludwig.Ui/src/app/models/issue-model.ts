import {IssueTypeModel} from "./issue-type-model";
import {ProjectModel} from "./project-model";
import {PriorityModel} from "./priority-model";
import {JiraUserModel} from "./jira-user-model";


export class IssueModel{

  id:number=0;
  self:string="";
  key:string="";
  issueType:IssueTypeModel;
  summary:string="";
  description:string="";
  project:ProjectModel;
  priority:PriorityModel;
  assignee:JiraUserModel;
  userStory:string="";
}
