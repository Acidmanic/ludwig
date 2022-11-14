import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {UserStoryModel} from "../../models/user-story.model";
import {UserStoryService} from "../../services/user-story/user-story.service";
import {ModalDismissReasons, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {WaiterService} from "../../services/waiter.service";


@Component({
  selector: 'app-shoe-box',
  templateUrl: './shoe-box.component.html',
  styleUrls: ['./shoe-box.component.css']
})
export class ShoeBoxComponent implements OnInit {


  stories:UserStoryModel[]=[];
  editorTitle:string='Create';
  editingStory:UserStoryModel=new UserStoryModel();
  @ViewChild('content') content:any;

  constructor(private svcStory:UserStoryService,
              private modalService: NgbModal,
              private svcWaiter:WaiterService) {
  }

  ngOnInit(): void {

    this.svcWaiter.start();

    this.svcStory.getAllStories().subscribe({
      next:stories => {
        this.stories = stories;
        console.log("stories:",this.stories);
        this.svcWaiter.stop();
      },
      error:err=>{
        this.svcWaiter.stop();
      },
      complete:()=>{
        this.svcWaiter.stop();
      }
    });

    console.log(this.content);
  }


  ngAfterViewInit() {
    console.log(this.content.nativeElement);
  }

  edit(story:UserStoryModel){

    this.editingStory = story;
    this.openModal('Edit ' + story.title);

  }

  createStory(){

    this.editingStory = new UserStoryModel();

    this.openModal('Create New Story');

    this.svcWaiter.start();
  }

  openModal(title:string) {

    this.editorTitle=title;

    this.modalService.open(this.content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
      (result) => {
        console.log("closed",result);
        console.log(this.editingStory);
      },
      (reason) => {
        console.log("pis pissed!");
      },
    );
  }


  caption(story:UserStoryModel){
    return 'As ' +
      story.storyUser.name + ', I Want ' +
      story.storyFeature+ ', So that I can ' +
      story.storyBenefit;
  }
}
