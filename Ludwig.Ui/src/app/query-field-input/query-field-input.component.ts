import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {QueryInputModel} from "../models/query-input-model";

@Component({
  selector: 'query-field-input',
  templateUrl: './query-field-input.component.html',
  styleUrls: ['./query-field-input.component.css']
})
export class QueryFieldInputComponent implements OnInit {


  @Input('query-input') queryInput:QueryInputModel = new QueryInputModel();
  @Output('query-value') queryValue:EventEmitter<string> = new EventEmitter<string>();
  @Output('value-received') valueReceived:EventEmitter<boolean> = new EventEmitter<boolean>();
  value:string='';
  received:boolean=false;

  constructor(private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.route.queryParams
      .subscribe(params => {
        this.value = params[this.queryInput.queryKey];
        this.queryValue.emit(this.value);
        this.received = false;
        if(this.value && this.value.trim().length>0){
          this.received = true;
        }
        this.valueReceived.emit(this.received);
      });
  }

}
