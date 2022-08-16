import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-calendar-contextmenu',
  templateUrl: './calendar-contextmenu.component.html',
  styleUrls: ['./calendar-contextmenu.component.css']
})
export class CalendarContextmenuComponent implements OnInit {

  constructor() { }

  @Input() isAdd?: boolean;
  @Output() showDialog = new EventEmitter<string>();

  ngOnInit(): void {
  }

  openDialog(operation: string){
    this.showDialog.emit(operation);
  }
}
