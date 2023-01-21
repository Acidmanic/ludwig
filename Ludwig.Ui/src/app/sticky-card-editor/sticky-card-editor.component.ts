import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CardModel} from "../models/card-model";

@Component({
  selector: 'sticky-card-editor',
  templateUrl: './sticky-card-editor.component.html',
  styleUrls: ['./sticky-card-editor.component.css']
})
export class StickyCardEditorComponent implements OnInit {

  @Input('card') card:CardModel=new CardModel();
  @Output('cardChange') onCardChange:EventEmitter<CardModel> = new EventEmitter<CardModel>();
  @Input('parents') parents:CardModel[]=[];
  @Output('parent-changed') onParentChanged:EventEmitter<CardModel>=new EventEmitter<CardModel>();
  @Input('child-type-name') childTypeName!:string;
  @Output('child-added') onChildAdded:EventEmitter<CardModel>=new EventEmitter<CardModel>();
  @Output('deleted') onDeleted:EventEmitter<CardModel>= new EventEmitter<CardModel>();

  constructor() { }

  ngOnInit(): void {
  }


  parentSelected(parent:CardModel){

    console.log('Parent selected: ',parent);
  }
}
