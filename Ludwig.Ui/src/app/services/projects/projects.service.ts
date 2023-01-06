import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProjectModel} from "../../models/project-model";
import {Observable, Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  constructor(private http:HttpClient) { }

  public baseUrl:string='';


  public getAllProjects():Observable<ProjectModel[]>{

    let url = this.baseUrl + "projects";

    let handle = new Subject<ProjectModel[]>();

    this.http.get<{projects:ProjectModel[]}>(url).subscribe({
      next: wrap => handle.next(wrap.projects),
      error: err => handle.error(err),
      complete: () => handle.complete()
    });

    return handle;

  }
}
