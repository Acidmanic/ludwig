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
  public isDone:boolean=false;

  public static loadInto(dst:UserStoryModel,src:any){
    let model = src as UserStoryModel;

    if(model){
      dst.id=model.id;
      dst.priority= {...model.priority};
      dst.isDone=model.isDone;
      dst.storyBenefit=model.storyBenefit;
      dst.storyFeature=model.storyFeature;
      dst.storyUser={...model.storyUser};
      dst.title=model.title;
      dst.issues=model.issues;
      dst.cardColor=model.cardColor;
    }
  }
}
