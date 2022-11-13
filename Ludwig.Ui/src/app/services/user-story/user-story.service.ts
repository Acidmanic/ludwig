import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, Subject} from "rxjs";
import {UserStoryModel} from "../../models/user-story.model";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class UserStoryService {


  private baseUrl=environment.baseUrl;

  constructor(private http:HttpClient) { }


  public getStoryById(id:number):Observable<UserStoryModel>{

    let handle = new Subject<UserStoryModel>();

    let url = this.baseUrl+"stories/"+id;

    this.http.get<UserStoryModel>(url).subscribe({
      next: model => handle.next(model),
      error:handle.error,
      complete:handle.complete
    });

    return handle;
  }

  public getAllStories():Observable<UserStoryModel[]>{

    let handle = new Subject<UserStoryModel[]>();

    let url = this.baseUrl+"stories";

    this.http.get<UserStoryModel[]>(url).subscribe({
      next: model => handle.next(model),
      error:handle.error,
      complete:handle.complete
    });

    return handle;
  }

}
