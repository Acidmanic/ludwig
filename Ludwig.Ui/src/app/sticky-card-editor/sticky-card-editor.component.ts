import {Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild, ViewRef} from '@angular/core';
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
  @Output('add-child-clicked') addChildClicked:EventEmitter<CardModel>=new EventEmitter<CardModel>();
  @Output('delete-clicked') deleteClicked:EventEmitter<CardModel>= new EventEmitter<CardModel>();

  constructor() { }

  ngOnInit(): void {
  }

  onParentSelected(event:any){
    let parent = event as CardModel;

    if(parent){
      this.onParentChanged.emit(parent);
    }
  }


  cardCaptioner(card:any){
      let c = card as CardModel;

      if(c){
        return c.title;
      }
      return 'Not a card: ' + card;
  }

}
