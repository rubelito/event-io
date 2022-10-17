import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-calendar-delete-dialog',
  templateUrl: './calendar-delete-dialog.component.html',
  styleUrls: ['./calendar-delete-dialog.component.css', '../../styles/style.css']
})
export class CalendarDeleteDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CalendarDeleteDialogComponent>) { }

  ngOnInit(): void {
  }

  onDeleteOne(){
    this.dialogRef.close("deleteOne");
  }
  
  onDeleteAll(){
    this.dialogRef.close("deleteAll");
  }

  onCancel(){
    this.dialogRef.close("cancel");
  }
}
