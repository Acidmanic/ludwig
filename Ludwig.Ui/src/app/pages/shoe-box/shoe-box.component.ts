import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {UserStoryModel} from "../../models/user-story.model";
import {UserStoryService} from "../../services/user-story/user-story.service";
import {ModalDismissReasons, NgbModal} from "@ng-bootstrap/ng-bootstrap";


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
              private modalService: NgbModal) {
  }

  ngOnInit(): void {

    this.svcStory.getAllStories().subscribe({
      next:stories => {
        this.stories = stories;
        console.log("stories:",this.stories);
      },
      error:err=>{},
      complete:()=>{}
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


}
