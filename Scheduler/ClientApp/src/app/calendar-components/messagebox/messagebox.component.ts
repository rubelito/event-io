import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Appointment } from 'src/app/calendar-models/Appointment';

@Component({
  selector: 'app-messagebox',
  templateUrl: './messagebox.component.html',
  styleUrls: ['./messagebox.component.css']
})
export class MessageboxComponent implements OnInit {
  @Input() appointment: Appointment;

  @Output() onOk = new EventEmitter();
  @Output() onCancel = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  cancel(){
    this.onCancel.emit();
  }

  ok(){
    this.onOk.emit();
  }
}
