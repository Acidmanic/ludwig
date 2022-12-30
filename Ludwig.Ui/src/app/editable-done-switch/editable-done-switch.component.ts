import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'editable-done-switch',
  templateUrl: './editable-done-switch.component.html',
  styleUrls: ['./editable-done-switch.component.css']
})
export class EditableDoneSwitchComponent implements OnInit {


  @Input('isDone') isDone:boolean=false;
  @Output('isDoneChange') isDoneChange:EventEmitter<boolean>=new EventEmitter<boolean>();
  @Output('on-update') onUpdate:EventEmitter<boolean>=new EventEmitter<boolean>();

  private originalDoneState:boolean=false;

  constructor() { }

  ngOnInit(): void {
    this.originalDoneState=this.isDone;
  }


  onClick(){
    this.isDone = !this.isDone;
    this.isDoneChange.emit(this.isDone);
    this.onUpdate.emit(this.isDone!=this.originalDoneState);
  }
}
