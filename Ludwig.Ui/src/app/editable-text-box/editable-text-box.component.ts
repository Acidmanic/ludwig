import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';

@Component({
  selector: 'editable-text-box',
  templateUrl: './editable-text-box.component.html',
  styleUrls: ['./editable-text-box.component.css']
})
export class EditableTextBoxComponent implements OnInit {

  @Input('text') text:string="";
  @Output('textChange') textChange:EventEmitter<string>=new EventEmitter<string>();
  @Output('updatedChange') updatedChange:EventEmitter<boolean>=new EventEmitter<boolean>();
  @ViewChild('textInput') textInput:any;

  editing:boolean=false;
  original:string="";
  private lastUpdatedStatus:boolean=false;
  constructor() { }

  ngOnInit(): void {
    this.original=this.text;
  }

  onText(text:string){
    this.text=text;
    this.textChange.emit(text);
  }

  toggleEdit(){
    this.editing=!this.editing;

    if(this.editing){

      setTimeout(()=>{ // this will make the execution after the above boolean has changed
        this.textInput.nativeElement.focus();
      },0);
    }else{
      let up = this.original!==this.text;
      if(up!=this.lastUpdatedStatus){
        this.lastUpdatedStatus=up;
        this.updatedChange.emit(up);
      }
    }
  }
}
