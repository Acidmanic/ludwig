import {Injectable} from "@angular/core";
import {Observable, Subject} from "rxjs";
import {LoginMethodModel} from "../../models/login-method-model";
import {HttpClient} from "@angular/common/http";
import {TokenModel} from "../../models/token-model";


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


  public login(model:object,methodName:string):Observable<TokenModel>{

    let handle = new Subject<TokenModel>();

    let url = 'auth/login' ;

    this.http.post<TokenModel>(url,{
      parameters:model,
      loginMethodName:methodName
    }).subscribe({
      next: tokenModel => handle.next(tokenModel),
      error: err => handle.error(err),
      complete:() => handle.complete()
    });

    return handle;

  }


}
