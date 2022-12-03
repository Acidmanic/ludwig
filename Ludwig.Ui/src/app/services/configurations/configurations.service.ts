import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, Subject} from "rxjs";
import {ConfigurationItemModel} from "../../models/configuration-item-model";
import {ConfigurationUpdateModel} from "../../models/configuration-update-model";

@Injectable({
  providedIn: 'root'
})
export class ConfigurationsService {

  constructor(private http:HttpClient) { }


  public read():Observable<ConfigurationItemModel[]>{

    let handler = new Subject<ConfigurationItemModel[]>();

    let url = 'configurations';

    this.http.get<ConfigurationItemModel[]>(url).subscribe({

      next: items => handler.next(items),
      error: err => handler.error(err),
      complete: () => handler.complete()
    });

    return handler;
  }

  public update(updateItems:ConfigurationItemModel[]):Observable<ConfigurationUpdateModel>{

    let handler = new Subject<ConfigurationUpdateModel>();

    let url = 'configurations';

    this.http.put<ConfigurationUpdateModel>(url,updateItems).subscribe({
      next: update => handler.next(update),
      error: err => handler.error(err),
      complete: () => handler.complete()
    });

    return handler;
  }
}
