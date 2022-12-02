import {Component, Input, OnInit} from '@angular/core';
import {ConfigurationItemModel} from "../models/configuration-item-model";

@Component({
  selector: 'editable-configuration-item',
  templateUrl: './editable-configuration-item.component.html',
  styleUrls: ['./editable-configuration-item.component.css']
})
export class EditableConfigurationItemComponent implements OnInit {

  @Input('configuration-item') configurationItem:ConfigurationItemModel=new ConfigurationItemModel();

  constructor() { }

  ngOnInit(): void {

  }


}
