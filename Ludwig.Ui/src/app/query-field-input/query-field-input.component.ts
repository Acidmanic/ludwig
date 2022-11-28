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

  value:string='';

  constructor(private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.route.queryParams
      .subscribe(params => {
        this.value = params[this.queryInput.queryKey];
        this.queryValue.emit(this.value);
      });
  }

}
