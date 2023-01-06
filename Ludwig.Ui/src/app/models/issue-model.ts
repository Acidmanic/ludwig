import {IssueTypeModel} from "./issue-type-model";
import {JiraProjectModel} from "./jira-project-model";
import {JiraPriorityModel} from "./jira-priority-model";
import {IssueManagerUserModel} from "./issue-manager-user-model";
import {PriorityModel} from "./priority-model";


export class IssueModel{

  issueReferenceLink:string="";
  issueType:IssueTypeModel=new IssueTypeModel();
  title:string="";
  description:string="";
  project:JiraProjectModel=new JiraProjectModel();
  priority:PriorityModel=new PriorityModel();
  assignees:IssueManagerUserModel[]=[];
  userStory:string="";
}
