import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ResultOf} from "../../models/result-of";
import {JiraUserModel} from "../../models/jira-user-model";
import {Observable, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class JiraService {

  constructor(private http:HttpClient) { }

  public loggedIn():Observable<ResultOf<JiraUserModel>>{

    let handle = new Subject<ResultOf<JiraUserModel>>();

    let url = "jira/logged-user"
    this.http.get<ResultOf<JiraUserModel>>(url).subscribe(
      {
        next: result => handle.next(result),
        error: err => handle.error(err),
        complete: () => handle.complete()
      }
    );

    return handle;
  }

  public logInByCredentials(username:string,password:string):Observable<ResultOf<JiraUserModel>>{

    let handle = new Subject<ResultOf<JiraUserModel>>();

    let url = "auth/jira/login"
    this.http.post<ResultOf<JiraUserModel>>(url,{username:username,password:password}).subscribe(
      {
        next: result => handle.next(result),
        error: err => handle.error(err),
        complete: () => handle.complete()
      }
    );

    return handle;
  }

}
