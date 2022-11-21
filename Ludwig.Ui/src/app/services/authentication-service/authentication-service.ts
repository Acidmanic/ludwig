import {Injectable} from "@angular/core";
import {Observable, Subject} from "rxjs";
import {LoginMethodModel} from "../../models/login-method-model";
import {HttpClient} from "@angular/common/http";


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService{

  constructor(private http:HttpClient) {
  }

  public getLoginMethods():Observable<LoginMethodModel[]>{

    let handle = new Subject<LoginMethodModel[]>();

    let url = 'auth/login-methods' ;

    this.http.get<{loginMethods:LoginMethodModel[]}>(url).subscribe({
      next: methods => handle.next(methods.loginMethods),
      error: err => handle.error(err),
      complete:() => handle.complete()
    });

    return handle;
  }


}
