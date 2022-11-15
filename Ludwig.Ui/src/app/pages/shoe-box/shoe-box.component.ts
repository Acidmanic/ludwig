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

  editOperation=()=>{};
  @ViewChild('content') content:any;

  constructor(private svcStory:UserStoryService,
              private modalService: NgbModal,
              private svcWaiter:WaiterService) {
  }

  ngOnInit(): void {
    this.refreshShoeBox();
  }


  ngAfterViewInit() {
  }

  // editStory(story:UserStoryModel){
  //
  //   this.editingStory = {
  //     ...story,
  //     storyUser:{
  //       ...story.storyUser
  //     }
  //   };
  //   this.editOperation = this.onUpdateEditingStory;
  //   // this.openModal('Edit ' + story.title);
  // }

  createStory(){
    let editingStory = EditableUserStoryComponent.blankStory();

    this.stories.push(editingStory);
  }

  // openModal(title:string) {
  //
  //   this.editorTitle=title;
  //
  //   this.modalService.open(this.content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
  //     (result) => {
  //     },
  //     (reason) => {
  //     },
  //   );
  // }

  askDeletingStory(story:UserStoryModel){
    this.deletingStory=story;
    this.messageBoxHook.emit();
  }

  deleteStory(story:UserStoryModel){

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


  refreshShoeBox(){

    this.svcWaiter.start();

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

  // onUpdateEditingStory(){
  //
  //   this.svcWaiter.start();
  //
  //   this.svcStory.updateStory(this.editingStory).subscribe({
  //     next:updated => {
  //       this.svcWaiter.stop();
  //       this.refreshShoeBox();
  //     },
  //     error:err=>{
  //       this.svcWaiter.stop();
  //     },
  //     complete:()=>{
  //       this.svcWaiter.stop();
  //     }
  //   });
  // }

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

  // onAddEditingStory(){
  //
  //   this.svcWaiter.start();
  //
  //   this.svcStory.addStory(this.editingStory).subscribe({
  //     next:inserted => {
  //       this.svcWaiter.stop();
  //       this.refreshShoeBox();
  //     },
  //     error:err=>{
  //       this.svcWaiter.stop();
  //     },
  //     complete:()=>{
  //       this.svcWaiter.stop();
  //     }
  //   });
  // }
  //
  // addStory(story:UserStoryModel){
  //
  //   this.svcWaiter.start();
  //
  //   this.svcStory.addStory(story).subscribe({
  //     next:inserted => {
  //       this.svcWaiter.stop();
  //       this.refreshShoeBox();
  //     },
  //     error:err=>{
  //       this.svcWaiter.stop();
  //     },
  //     complete:()=>{
  //       this.svcWaiter.stop();
  //     }
  //   });
  // }

  caption(story:UserStoryModel){
    return 'As ' +
      story.storyUser.name + ', I Want ' +
      story.storyFeature+ ', So that I can ' +
      story.storyBenefit;
  }


}
