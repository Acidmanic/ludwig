import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'message-box',
  templateUrl: './message-box.component.html',
  styleUrls: ['./message-box.component.css']
})
export class MessageBoxComponent implements OnInit {

  @Input('title') modalTitle:string='Hi!';
  @Input('caption') modalText:string='Do you agree?';
  @Input('accept-button') accCaption:string='Yes';
  @Input('reject-button') rejCaption:string='No';
  @Input('can-reject') canReject:boolean=true;
  @Input('negative-reject') negativeReject:boolean=true;
  @Input('show-hook') showHook= new EventEmitter();
  @Output('accepted') acceptEvent= new EventEmitter();

  @Output('rejected') rejectedEvent= new EventEmitter();
  @ViewChild('content') content:any;


  @Input('positive-class')positiveButtonClass:string='btn-success';
  @Input('negative-class')negativeButtonClass:string='btn-danger';

  acceptButtonClass:string='';
  rejectButtonClass:string='';


  constructor(private modalService: NgbModal) { }

  ngOnInit(): void {
    this.showHook.subscribe({
      next: () => this.show()
    });

  }


  yesIt(){
    this.acceptEvent.emit();
  }

  noIt(){
    this.rejectedEvent.emit();
  }

  show(){
    if(this.negativeReject){
      this.acceptButtonClass=this.positiveButtonClass;
      this.rejectButtonClass=this.negativeButtonClass;
    }else{
      this.acceptButtonClass=this.negativeButtonClass;
      this.rejectButtonClass=this.positiveButtonClass;
    }
    this.modalService.open(this.content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
      (result) => {
      },
      (reason) => {
      },
    );
  }
}
