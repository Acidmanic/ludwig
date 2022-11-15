import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'collapsible',
  templateUrl: './collapsible.component.html',
  styleUrls: ['./collapsible.component.css']
})
export class CollapsibleComponent implements OnInit {

  constructor() { }

  @Input('collapsed') collapsed:boolean=true;
  @Output('collapsedChange') collapsedChange:EventEmitter<boolean>=new EventEmitter<boolean>();
  @Input('alternative-text') alternativeText:string='...';


  ngOnInit(): void {
  }


  toggle(){
    this.collapsed=!this.collapsed;
    this.collapsedChange.emit(this.collapsed);
  }
}
