import {Component, EventEmitter, Input, OnInit, Output, Renderer2} from '@angular/core';
import {UserStoryModel} from "../models/user-story.model";
import {StoryUserModel} from "../models/story-user.model";
import {Trigger} from "../utilities/trigger";
import {IssueModel} from "../models/issue-model";
import {PriorityModel} from "../models/priority-model";
import {Priorities} from "../services/Priorities";

@Component({
  selector: 'editable-user-story',
  templateUrl: './editable-user-story.component.html',
  styleUrls: ['./editable-user-story.component.css']
})
export class EditableUserStoryComponent implements OnInit {


  @Input('story') story:UserStoryModel=EditableUserStoryComponent.blankStory();
  @Input('force-enable-sync') forceEnableSync:boolean=false;
  @Output('storyChange') storyChange:EventEmitter<UserStoryModel> = new EventEmitter<UserStoryModel>();
  @Output('syncStory') syncStory:EventEmitter<UserStoryModel> = new EventEmitter<UserStoryModel>();
  @Output('deleteStory') deleteStory:EventEmitter<UserStoryModel> = new EventEmitter<UserStoryModel>();
  @Output('on-issue') onIssue:EventEmitter<IssueModel> = new EventEmitter<IssueModel>();

  original:UserStoryModel=new UserStoryModel();

  private updateFields:boolean[]=[false,false,false,false,false,false];
  modelUpdate:boolean=false;
  priorityRevertTrigger:Trigger=new Trigger();
  newIssueForm:EventEmitter<any>=new EventEmitter<any>();
  newIssue:IssueModel=new IssueModel();

  constructor() { }


  ngOnInit(): void {
    this.original={
      ...this.story,
      storyUser:{
        ...this.story.storyUser
      },
      priority:{
        ...this.story.priority
      }
    };
  }

  public static blankStory():UserStoryModel{
    let story = new UserStoryModel();
    story.storyBenefit="Somehow Benefit from it";
    story.storyFeature="Specific Feature";
    story.storyUser=new StoryUserModel();
    story.storyUser.name="An Imaginary User";
    story.title="Example";
    story.priority={name:'Medium',value:2};
    return story;
  }


  onFieldUpdate(index:number,update:boolean){

    console.log('Update field:',index,update);

    this.updateFields[index]=update;

    var modelUpdate:boolean= false;

    this.updateFields.forEach( up => modelUpdate = modelUpdate || up);

    this.modelUpdate=modelUpdate;

    if(this.modelUpdate){
      this.storyChange.emit(this.story);
    }
  }

  onSyncClick(){
    this.syncStory.emit(this.story);
  }

  onRevertClick(){

    this.story={
      ...this.original,
      storyUser:{
        ...this.original.storyUser
      },
      priority:{
        ...this.original.priority
      }
    };

    this.updateFields=[false,false,false,false,false];
    this.modelUpdate=false;
    this.storyChange.emit(this.story);
    this.priorityRevertTrigger.fire();
  }

  onDeleteClick(){

    this.deleteStory.emit(this.story);
  }

  onNewIssueButtonClicked(){
    this.newIssue = new IssueModel();
    this.newIssue.title=this.story.storyFeature;
    this.newIssue.userStory=this.story.title;
    this.newIssue.description='As ' + this.story.storyUser.name + ' I want ' + this.story.storyFeature
      + ', so that i can ' + this.story.storyBenefit;
    this.newIssue.priority=this.story.priority;

    this.newIssueForm.emit();
  }

  onNewIssueAccepted(){

    console.log(this.newIssue);
    this.onIssue.emit(this.newIssue);
  }
}
