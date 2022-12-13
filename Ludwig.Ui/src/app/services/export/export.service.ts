import { Injectable } from '@angular/core';
import {environment} from "../../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable, Subject} from "rxjs";
import {UserStoryModel} from "../../models/user-story.model";
import {ExportInfoModel} from "../../models/export-info-model";

@Injectable({
  providedIn: 'root'
})
export class ExportService {


  private baseUrl=environment.baseUrl;

  constructor(private http:HttpClient) { }


  public getAvailableExports():Observable<ExportInfoModel[]>{

    let handle = new Subject<ExportInfoModel[]>();

    let url = this.baseUrl+"export";

    this.http.get<ExportInfoModel[]>(url).subscribe({
      next: model => handle.next(model),
      error:err=>handle.error(err),
      complete:()=>handle.complete()
    });

    return handle;
  }

}
