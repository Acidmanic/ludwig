import {Component, Input, OnInit} from '@angular/core';
import {IssueModel} from "../models/issue-model";

@Component({
  selector: 'issue-item',
  templateUrl: './issue-item.component.html',
  styleUrls: ['./issue-item.component.css']
})
export class IssueItemComponent implements OnInit {

  constructor() { }

  @Input('issue') issue:IssueModel=new IssueModel();


  ngOnInit(): void {
  }

}
