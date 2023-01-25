import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CardModel} from "../models/card-model";

@Component({
  selector: 'sticky-card',
  templateUrl: './sticky-card.component.html',
  styleUrls: ['./sticky-card.component.css']
})
export class StickyCardComponent implements OnInit {


  @Input('card') card:CardModel=new CardModel();
  @Output('cardChange') cardChange:EventEmitter<CardModel>=new EventEmitter<CardModel>();
  @Input('card-type-class') cardTypeClass:string='';

  constructor() { }

  ngOnInit(): void {
  }

  captioned():boolean{
    if(this.card.description){
      if(this.card.description.trim().length>0){
        return true;
      }
    }
    return false;
  }
}
