import { Component, OnInit } from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {WaiterService} from "../services/waiter.service";

@Component({
  selector: 'waiting-logo',
  templateUrl: './waiting-logo.component.html',
  styleUrls: ['./waiting-logo.component.css'],
  animations: [
    // Each unique animation requires its own trigger. The first argument of the trigger function is the name
    trigger('rotatedState', [
      state('default', style({ transform: 'rotate(0)' })),
      state('rotated', style({ transform: 'rotate(-180deg)' })),
      transition('rotated => default', animate('1500ms ease-out')),
      transition('default => rotated', animate('400ms ease-in'))
    ])
  ]
})
export class WaitingLogoComponent implements OnInit {

  constructor() { }

  state: string = 'default';


  ngOnInit(): void {
    WaiterService.disableFunction=() => this.state = 'default';
    WaiterService.enableFunction=() => this.state = 'rotated';
    WaiterService.toggle=() => this.state = (this.state === 'default' ? 'rotated' : 'default');
  }

}
