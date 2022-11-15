import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {UserStoryModel} from "../models/user-story.model";
import {StoryUserModel} from "../models/story-user.model";

@Component({
  selector: 'editable-user-story',
  templateUrl: './editable-user-story.component.html',
  styleUrls: ['./editable-user-story.component.css']
})
export class EditableUserStoryComponent implements OnInit {


  @Input('story') story:UserStoryModel=EditableUserStoryComponent.blankStory();
  @Output('storyChange') storyChange:EventEmitter<UserStoryModel> = new EventEmitter<UserStoryModel>();
  @Output('syncStory') syncStory:EventEmitter<UserStoryModel> = new EventEmitter<UserStoryModel>();

  original:UserStoryModel=new UserStoryModel();

  private updateFields:boolean[]=[false,false,false,false];
  modelUpdate:boolean=false;

  constructor() { }


  ngOnInit(): void {
    this.original={
      ...this.story,
      storyUser:{
        ...this.story.storyUser
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
      }
    };
    this.updateFields=[false,false,false,false];
    this.modelUpdate=false;
    this.storyChange.emit(this.story);
  }
}
