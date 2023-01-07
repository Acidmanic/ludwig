import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {CardModel} from "../models/card-model";

@Component({
  selector: 'app-sticky-card',
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

}
