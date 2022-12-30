import { Component, OnInit } from '@angular/core';
import {ConfigurationItemModel} from "../../models/configuration-item-model";
import {ConfigurationsService} from "../../services/configurations/configurations.service";
import {WaiterService} from "../../services/waiter.service";
import {MessageModel} from "../../models/message-model";

@Component({
  selector: 'configurations',
  templateUrl: './configurations.component.html',
  styleUrls: ['./configurations.component.css']
})
export class ConfigurationsComponent implements OnInit {


  configurationItems:ConfigurationItemModel[]=[];
  receivedMessage:MessageModel=new MessageModel();


  constructor(private svcConf:ConfigurationsService,
              private svcWait:WaiterService) { }

  ngOnInit(): void {

    this.readAllConfigurations();

  }

  readAllConfigurations(){

    this.svcWait.start();

    this.svcConf.read().subscribe({
      next: items => {
        this.configurationItems = items;
        this.svcWait.stop();
      },
      error: err => {
        this.svcWait.stop();
      },
      complete: () => {
        this.svcWait.stop();
      }
    });
  }

  onSaveClick(){

    this.svcWait.start();

    this.receivedMessage = new MessageModel();

    this.svcConf.update(this.configurationItems).subscribe({
      next: update => {
        this.svcWait.stop();
        this.configurationItems = update.items;
        this.receivedMessage = update.message;
      },
      error: err => this.svcWait.stop(),
      complete: () => this.svcWait.stop()
    });

  }

}
