import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, Subject} from "rxjs";
import {ConfigurationItemModel} from "../../models/configuration-item-model";

@Injectable({
  providedIn: 'root'
})
export class ConfigurationsService {

  constructor(private http:HttpClient) { }




  public getAll():Observable<ConfigurationItemModel[]>{

    let handler = new Subject<ConfigurationItemModel[]>();

    let url = 'configurations';

    this.http.get<ConfigurationItemModel[]>(url).subscribe({

      next: items => handler.next(items),
      error: err => handler.error(err),
      complete: () => handler.complete()
    });

    return handler;
  }

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

  public update(updateItems:ConfigurationItemModel[]):Observable<ConfigurationItemModel[]>{

    let handler = new Subject<ConfigurationItemModel[]>();

    let url = 'configurations';

    this.http.put<ConfigurationItemModel[]>(url,updateItems).subscribe({
      next: items => handler.next(items),
      error: err => handler.error(err),
      complete: () => handler.complete()
    });

    return handler;
  }
}
