import { Component, OnInit } from '@angular/core';
import {WaiterService} from "../services/waiter.service";

@Component({
  selector: 'disabler',
  templateUrl: './disabler.component.html',
  styleUrls: ['./disabler.component.css']
})
export class DisablerComponent implements OnInit {

  public block:boolean=false;
  public styleString:string='#000';

  constructor(private svcWait:WaiterService) { }



  ngOnInit(): void {
    this.svcWait.state().subscribe({
      next:goodies => {
        this.block=goodies.waiting;
        this.styleString='background-color: '+goodies.color.colorCode;
      }
    });
  }

}
