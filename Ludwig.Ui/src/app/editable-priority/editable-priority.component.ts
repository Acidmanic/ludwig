import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {PriorityModel} from "../models/priority-model";
import {UserStoryModel} from "../models/user-story.model";
import {Trigger} from "../utilities/trigger";

@Component({
  selector: 'editable-priority',
  templateUrl: './editable-priority.component.html',
  styleUrls: ['./editable-priority.component.css']
})
export class EditablePriorityComponent implements OnInit {

  @Input('priority') priority:PriorityModel=new PriorityModel();
  @Output('priorityChange') priorityChange:EventEmitter<PriorityModel>=new EventEmitter<PriorityModel>();
  @Input('onRevert') onRevert:Trigger =new Trigger();
  @Output('updated-change') updatedChange:EventEmitter<boolean>=new EventEmitter<boolean>();

  priorities:PriorityModel[]=[
    {name:'Highest',value:0},
    {name:'High',value:1},
    {name:'Medium',value:2},
    {name:'Low',value:3},
    {name:'Lowest',value:4},
  ];
  private original:PriorityModel=new PriorityModel();
  private lastChangedStatus:boolean=false;
  constructor() { }

  ngOnInit(): void {
    this.original={
      ...this.priority
    };
    this.onRevert.subscribe(() => {

      this.revert();

      this.checkUpdateStatus();
    });
  }

  revert(){
    this.priority={...this.original};
  }

  changePriority(){
    let index = this.priority.value;
    index++;
    if(index>=this.priorities.length){
      index=0;
    }
    this.priority=this.priorities[index];

    this.priorityChange.emit(this.priority);

    this.checkUpdateStatus();
  }

  checkUpdateStatus(){
    let changed = this.original.value!=this.priority.value || this.original.name!=this.priority.name;

    if(changed!=this.lastChangedStatus){
      this.lastChangedStatus=changed;
      this.updatedChange.emit(changed);
    }
  }
}
