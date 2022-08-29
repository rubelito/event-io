import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogOperation } from 'src/app/calendar-models/DialogOperation';
import { CalendarDialogComponent } from '../calendar-dialog/calendar-dialog.component';

@Component({
  selector: 'app-calendar-view-dialog',
  templateUrl: './calendar-view-dialog.component.html',
  styleUrls: ['./calendar-view-dialog.component.css']
})
export class CalendarViewDialogComponent implements OnInit {

  event: any;

  constructor(public dialogRef: MatDialogRef<CalendarDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {param: DialogOperation}) { }

  ngOnInit(): void {
    this.event = this.data.param;
  }

  close(){
    this.dialogRef.close('close');
  }
}
