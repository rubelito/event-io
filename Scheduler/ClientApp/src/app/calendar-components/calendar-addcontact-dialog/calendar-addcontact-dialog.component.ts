import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth-service/AuthService';
import { MessageboxComponent } from '../messagebox/messagebox.component';

@Component({
  selector: 'app-calendar-addcontact-dialog',
  templateUrl: './calendar-addcontact-dialog.component.html',
  styleUrls: ['./calendar-addcontact-dialog.component.css', '../../styles/style.css']
})
export class CalendarAddContactDialogComponent implements OnInit {

  email: string = "";

  constructor(public dialogRef: MatDialogRef<CalendarAddContactDialogComponent>,
    private authService: AuthService, private dialog:MatDialog) { }

  ngOnInit(): void {
  }

  cancel() {
    this.dialogRef.close("cancel");
  }

  add(){
    this.authService.addContact(this.email).subscribe(result => {
      if (result.Success){
        let dialogResult =this.dialog.open(MessageboxComponent, {
          width:'400px',
          disableClose: true,
          data: { title: '', message: "Contact Added!", hasNoCancel: true, icon: "ok"}
        });

        dialogResult.afterClosed().subscribe(result => {
        this.dialogRef.close("add");
        });
      }
      else {
        this.showMessage(result.Message, "warning");
      }
    });
      }

  showMessage(message: string, icon: string){
    this.dialog.open(MessageboxComponent, {
      width:'400px',
      disableClose: true,
      data: { title: '', message: message, hasNoCancel: true, icon: icon}
    });
  }
}
