import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, Subject} from "rxjs";
import {UserStoryModel} from "../../models/user-story.model";

@Injectable({
  providedIn: 'root'
})
export class UserStoryService {



  constructor(private http:HttpClient) { }


  public getStoryById(id:number):Observable<UserStoryModel>{

    let handle = new Subject<UserStoryModel>();

    let url = "stories/"+id;

    this.http.get<UserStoryModel>(url).subscribe({
      next: model => handle.next(model),
      error:handle.error,
      complete:handle.complete
    });

    return handle;
  }

  public getAllStories():Observable<UserStoryModel[]>{

    let handle = new Subject<UserStoryModel[]>();

    let url = "stories";

    this.http.get<UserStoryModel[]>(url).subscribe({
      next: model => handle.next(model),
      error:handle.error,
      complete:handle.complete
    });

    return handle;
  }

}
