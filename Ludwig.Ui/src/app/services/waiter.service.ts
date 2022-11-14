import { Injectable } from '@angular/core';
import {Observable, Subject, Subscription, timer} from "rxjs";
import {Color} from "./color";
import {WaitGoodies} from "./wait-goodies";

@Injectable({
  providedIn: 'root'
})
export class WaiterService {

  public static enableFunction=()=>{};
  public static disableFunction=()=>{};
  public static toggle=()=>{};

  private static waiters:number=0;
  private static timerHandle:Subscription;
  private static stateChange:Subject<WaitGoodies>=new Subject<WaitGoodies>();

  private static color:Color=Color.create(90,121,83,0.3);
  private static goodies:WaitGoodies={color:WaiterService.color,waiting:false};

  constructor() {
    WaiterService.stateChange.next(WaiterService.goodies);

  }


  public state():Observable<WaitGoodies>{

    return WaiterService.stateChange;
  }

  public start(){

    WaiterService.waiters++;
    WaiterService.goodies.waiting=true;
    WaiterService.stateChange.next(WaiterService.goodies);
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
            WaiterService.toggle();
            WaiterService.goodies.color.incrementColor();
            WaiterService.stateChange.next(WaiterService.goodies);
            if(WaiterService.waiters<=0){
              WaiterService.goodies.waiting=false;
              WaiterService.stateChange.next(WaiterService.goodies);
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
