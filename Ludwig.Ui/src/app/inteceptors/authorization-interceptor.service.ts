import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse} from "@angular/common/http";
import {Observable, Subject} from "rxjs";
import {Injectable} from "@angular/core";
import {LoginManagerService} from "../services/login-manager/login-manager.service";
import {Router} from "@angular/router";


@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor{


  constructor(private svcLoginManager:LoginManagerService,
              private router:Router) {
  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // redirect check before the request being made
    if(!this.svcLoginManager.isLoggedIn) {
      if (!this.router.url.toLowerCase().startsWith("/login")) {
        this.redirect();
      }
    }

    var fwdRequest:HttpRequest<any>;

    if(this.svcLoginManager.isLoggedIn){
      fwdRequest = req.clone({
        setHeaders:{
          authorization:'token ' + this.svcLoginManager.token.token
        },
      });
    }else{
      fwdRequest = req;
    }

    let handle = new Subject<HttpEvent<any>>()

    next.handle(fwdRequest).subscribe({
      next: n => {
        let response = n as HttpResponse<any>;

        if(response && response.status==401){
          this.redirect()
        }
        handle.next(n);
      },
      error: er => {
        if(er.status==401){
          this.redirect();
        }
        handle.error(er);

      },
      complete: () => handle.complete()
    });

    return handle;
  }

  private redirect(): void
  {
    // to clear saved logins
    this.svcLoginManager.logOut();
    this.router.navigateByUrl('/login');
    console.log('Redirected because of 401',this.router.url.toLowerCase());
  }

}
