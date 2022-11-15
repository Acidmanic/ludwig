import { Component, OnInit } from '@angular/core';
import {WaiterService} from "../services/waiter.service";
import {animate, style, state, transition, trigger} from "@angular/animations";

@Component({
  selector: 'disabler',
  templateUrl: './disabler.component.html',
  styleUrls: ['./disabler.component.css'],
  animations: [
    // Each unique animation requires its own trigger. The first argument of the trigger function is the name
    trigger('backgroundState', [
      state('default', style({ backgroundColor: '#FFFFFF00' })),
      state('warm', style({ backgroundColor: '#AB55553A' })),
      state('magic', style({ backgroundColor: 'rgba(162,85,171,0.23)' })),
      state('safe', style({ backgroundColor: 'rgba(85,92,171,0.23)' })),
      state('positive', style({ backgroundColor: 'rgba(85,171,109,0.23)' })),
      state('happy', style({ backgroundColor: 'rgba(171,164,85,0.23)' })),
      transition('* => default', animate('1500ms ease-out')),
      transition('default => warm', animate('400ms ease-in')),
      transition('warm => magic', animate('400ms ease-in')),
      transition('magic => safe', animate('400ms ease-in')),
      transition('safe => positive', animate('400ms ease-in')),
      transition('positive => happy', animate('400ms ease-in')),
      transition('happy => warm', animate('400ms ease-in')),
    ])
  ]
})
export class DisablerComponent implements OnInit {

  public block:boolean=false;
  private stateSequence:string[]=['warm','magic','safe','positive','happy'];
  state:string='default';
  private stateIndex=0;
  constructor(private svcWait:WaiterService) { }



  ngOnInit(): void {
    this.svcWait.state().subscribe({
      next:goodies => {
        this.block=goodies.waiting;
        if(goodies.waiting){
          this.update();
          console.log(this.state);
        }else{
          this.stop();
        }
      }
    });
  }



  public update(){
    if(this.stateIndex>=this.stateSequence.length){
      this.stateIndex=0;
    }
    this.state=this.stateSequence[this.stateIndex];
    this.stateIndex++;
  }

  public stop(){
    this.stateIndex=0;
    this.state='default';
  }
}
