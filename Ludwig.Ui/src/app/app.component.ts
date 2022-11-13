import {Component, OnInit} from '@angular/core';
import {UserStoryModel} from "./models/user-story.model";
import {UserStoryService} from "./services/user-story/user-story.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  title = 'Ludwig.Ui';

  stories:UserStoryModel[]=[];

  constructor(private svcStory:UserStoryService) {
  }

  ngOnInit(): void {

    this.svcStory.getAllStories().subscribe({
      next:stories => {
        this.stories = stories;
        console.log("stories:",this.stories);
      }
    });

  }

}
