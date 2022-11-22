import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {LoginManagerService} from "../services/login-manager/login-manager.service";
import {Router} from "@angular/router";


@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor{


  constructor(private svcLoginManager:LoginManagerService,
              private router:Router) {
  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {


    if(this.svcLoginManager.isLoggedIn){
      let newReq = req.clone({
        setHeaders:{
          authorization:'token ' + this.svcLoginManager.token.token
        },
      });

      return next.handle(newReq);
    }else{
      this.router.navigateByUrl('/login');
    }
    return next.handle(req);
  }

}
