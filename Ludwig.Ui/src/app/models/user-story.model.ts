import {StoryUserModel} from "./story-user.model";
import {IssueModel} from "./issue-model";
import {PriorityModel} from "./priority-model";


export class UserStoryModel{


  public id:number =0;
  public title:string="";
  public storyUser:StoryUserModel=new StoryUserModel();
  public storyUserId:number=0;
  public storyFeature:string="";
  public storyBenefit:string="";
  public cardColor:string="gray";
  public issues:IssueModel[]=[];
  public priority:PriorityModel=new PriorityModel();

}
