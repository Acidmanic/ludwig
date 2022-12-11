import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, Subject} from "rxjs";
import {IssueManagerUserModel} from "../../models/issue-manager-user-model";
import {IssueModel} from "../../models/issue-model";

@Injectable({
  providedIn: 'root'
})
export class IssueManagerServiceService {

  constructor(private http:HttpClient) { }


  public getMe():Observable<IssueManagerUserModel>{

    let handler = new Subject<IssueManagerUserModel>();

    let url = "issue-manager/me";

    this.http.get<IssueManagerUserModel>(url).subscribe({
      next:user=>handler.next(user),
      error:err=>handler.error(err),
      complete:()=>handler.complete()
    });

    return handler;
  }

  public getMeBeforeLoggedIn(authorizationHeader:string):Observable<IssueManagerUserModel>{

    let handler = new Subject<IssueManagerUserModel>();

    let url = "issue-manager/me";

    this.http.get<IssueManagerUserModel>(url,{
      headers:{authorization:authorizationHeader}
    }).subscribe({
      next:user => handler.next(user),
      error:err=>handler.error(err),
      complete:()=>handler.complete()
    });

    return handler;
  }


  public createIssue(issue:IssueModel):Observable<IssueModel>{

    let handler = new Subject<IssueModel>();

    let url = "issue-manager/issues";

    this.http.post<IssueModel>(url,issue).subscribe({
      next:user => handler.next(user),
      error:err=>handler.error(err),
      complete:()=>handler.complete()
    });

    return handler;
  }
}
