import {StoryUserModel} from "./story-user.model";
import {IssueModel} from "./issue-model";


export class UserStoryModel{


  public id:number =0;
  public title:string="";
  public storyUser:StoryUserModel=new StoryUserModel();
  public storyUserId:number=0;
  public storyFeature:string="";
  public storyBenefit:string="";
  public cardColor:string="gray";
  public issues:IssueModel[]=[];

}
