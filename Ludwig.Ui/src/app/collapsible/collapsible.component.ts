import {Component, ElementRef, EventEmitter, Input, OnInit, Output, Renderer2, ViewChild} from '@angular/core';

@Component({
  selector: 'collapsible',
  templateUrl: './collapsible.component.html',
  styleUrls: ['./collapsible.component.css']
})
export class CollapsibleComponent implements OnInit {

  constructor(private renderer:Renderer2) { }

  @Input('collapsed') collapsed:boolean=true;
  @Output('collapsedChange') collapsedChange:EventEmitter<boolean>=new EventEmitter<boolean>();
  @Input('alternative-text') alternativeText:string='...';
  @Input('collapse-button-classes') colButCss:string="";
  @ViewChild('meView') meView:any;
  @Input('content-class') contentClass:string='';
  @Input('collapse-label-class') labelClass:string='';

  ngOnInit(): void {

    this.renderer.listen('body','click',(e:Event) => {

          var insideClick: boolean = false;

          e.composedPath().forEach(f => insideClick = insideClick || f == this.meView.nativeElement);

          if (!insideClick) {
            this.collapse();
          }
    });

  }


  toggle(){
    this.collapsed=!this.collapsed;
    this.collapsedChange.emit(this.collapsed);
  }

  collapse(){
    this.collapsed=true;
    this.collapsedChange.emit(this.collapsed);
  }
}
