import { Injectable } from '@angular/core';
import {Observable, Subscription, timer} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class WaiterService {

  public static enableFunction=()=>{};
  public static disableFunction=()=>{};
  public static toggle=()=>{};

  private static waiters:number=0;
  private static timerHandle:Subscription;

  constructor() {

  }

  public start(){

    WaiterService.waiters++;
    this.shootTimer()

  }

  private killTimer(){
    if(WaiterService.timerHandle){
      try {
        WaiterService.timerHandle.unsubscribe()
      }catch (e) {
      }
    }
  }

  private shootTimer(){
    this.killTimer();


    WaiterService.timerHandle = timer(0, 300)
        .subscribe({
          next: () => {
            console.log("shit",WaiterService.waiters);
            WaiterService.toggle();
            if(WaiterService.waiters<=0){
              console.log("im trying to die!",WaiterService.waiters);
              this.killTimer();
              WaiterService.disableFunction();
            }
          }
        });


  }

  public stop(){

    if(WaiterService.waiters>0){
      WaiterService.waiters--;
    }
  }
}
