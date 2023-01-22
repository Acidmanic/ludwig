import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'toggle-drop-select',
  templateUrl: './toggle-drop-select.component.html',
  styleUrls: ['./toggle-drop-select.component.css']
})
export class ToggleDropSelectComponent implements OnInit {


  @Input('title') title:string='Please select';
  @Input('items') items:any[]=[];
  @Input('captioner') captioner:(c:any)=>string=(c)=>c;

  @Input('ngModel') ngModel:any=null;
  @Output('ngModelChange') ngModelChange:EventEmitter<any>=new EventEmitter<any>();

  @Output('on-select') onSelect:EventEmitter<any>=new EventEmitter<any>();

  @Input('toggle-class') toggleClass:string='';
  @Input('drop-tray-class') dropTrayClass:string='';
  @Input('drop-item-class') dropItemClass:string='';

  constructor() { }

  ngOnInit(): void {
  }


  selected(item:any){

    this.ngModel=item;
    this.title=this.captioner(item);

    this.ngModelChange.emit(item);
    this.onSelect.emit(item);
  }
}
