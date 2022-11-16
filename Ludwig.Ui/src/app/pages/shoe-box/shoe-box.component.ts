import {Component, ElementRef, EventEmitter, OnInit, ViewChild} from '@angular/core';
import {UserStoryModel} from "../../models/user-story.model";
import {UserStoryService} from "../../services/user-story/user-story.service";
import {ModalDismissReasons, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {WaiterService} from "../../services/waiter.service";
import {StoryUserModel} from "../../models/story-user.model";
import {Observable} from "rxjs";
import {EditableUserStoryComponent} from "../../editable-user-story/editable-user-story.component";


@Component({
  selector: 'app-shoe-box',
  templateUrl: './shoe-box.component.html',
  styleUrls: ['./shoe-box.component.css']
})
export class ShoeBoxComponent implements OnInit {


  stories:UserStoryModel[]=[];
  editorTitle:string='Create';
  editingStory:UserStoryModel=new UserStoryModel();
  deletingStory:UserStoryModel=new UserStoryModel();
  messageBoxHook:EventEmitter<any> = new EventEmitter<any>();
  selectedStory?:UserStoryModel;


  editOperation=()=>{};

  constructor(private svcStory:UserStoryService,
              private modalService: NgbModal,
              private svcWaiter:WaiterService) {
  }

  ngOnInit(): void {
    this.refreshShoeBox();
  }


  ngAfterViewInit() {
  }

  createStory(){
    let editingStory = EditableUserStoryComponent.blankStory();

    this.stories.push(editingStory);
  }


  onSelectChanged(selected:boolean, story:UserStoryModel){
    if(this.selectedStory==story && !selected){
      this.selectedStory=undefined;
    }
    if(selected){
      this.selectedStory=story;
    }
  }

  duplicateStory(){
    let editingStory:UserStoryModel = {
      ...this.selectedStory!,
      id:0,
      storyUser:{
        ...this.selectedStory?.storyUser!
      },
      priority:{
        ...this.selectedStory?.priority!
      }
    } ;

    this.stories.push(editingStory);
  }
  askDeletingStory(story:UserStoryModel){
    this.deletingStory=story;
    this.messageBoxHook.emit();
  }

  deleteStory(story:UserStoryModel){

    var i:number=0;

    while(i<this.stories.length){

      let s = this.stories[i];

      if(s.id==story.id){
        this.stories.splice(i,1);
      }else{
        i++;
      }
    }

    if(this.selectedStory==story){
      this.selectedStory=undefined;
    }

    if(story.id>0){
      this.svcStory.deleteStory(story).subscribe({
        next:updated => {
          this.svcWaiter.stop();
          this.refreshShoeBox();
        },
        error:err=>{
          this.svcWaiter.stop();
        },
        complete:()=>{
          this.svcWaiter.stop();
        }
      });
    }
  }


  refreshShoeBox(){

    this.svcWaiter.start();

    this.selectedStory=undefined;

    this.svcStory.getAllStories().subscribe({
      next:stories => {
        this.stories = stories;
        this.svcWaiter.stop();
      },
      error:err=>{
        this.svcWaiter.stop();
      },
      complete:()=>{
        this.svcWaiter.stop();
      }
    });
  }


  saveUserStory(story:UserStoryModel){

    this.svcWaiter.start();

    let handle:Observable<UserStoryModel>;

    if(story.id>0){
      handle=this.svcStory.updateStory(story);
    }else{
      handle=this.svcStory.addStory(story);
    }

    handle.subscribe({
      next:updated => {
        this.svcWaiter.stop();
        this.refreshShoeBox();
      },
      error:err=>{
        this.svcWaiter.stop();
      },
      complete:()=>{
        this.svcWaiter.stop();
      }
    });
  }

  caption(story:UserStoryModel){
    return 'As ' +
      story.storyUser.name + ', I Want ' +
      story.storyFeature+ ', So that I can ' +
      story.storyBenefit;
  }


  glowStyle(story:UserStoryModel):string {
    return story.id==0?'newborn-glow':'';
  }


}
