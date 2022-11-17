import {Component, OnInit} from '@angular/core';
import {CookieService} from "ngx-cookie-service/dist-lib";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{


  title = 'Ludwig.Ui';

  constructor() {

  }


  ngOnInit(): void {

  }

}
