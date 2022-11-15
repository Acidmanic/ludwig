import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {UserStoryModel} from "../models/user-story.model";
import {StoryUserModel} from "../models/story-user.model";

@Component({
  selector: 'editable-user-story',
  templateUrl: './editable-user-story.component.html',
  styleUrls: ['./editable-user-story.component.css']
})
export class EditableUserStoryComponent implements OnInit {


  @Input('story') story:UserStoryModel=this.blankStory();
  @Output('storyChange') storyChange:EventEmitter<UserStoryModel> = new EventEmitter<UserStoryModel>();


  private updateFields:boolean[]=[false,false,false,false];
  modelUpdate:boolean=false;

  constructor() { }


  ngOnInit(): void {
  }

  blankStory():UserStoryModel{
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
}
