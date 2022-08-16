import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth-service/AuthService';
import { Role } from 'src/app/calendar-models/role';
import { UserBasic } from 'src/app/calendar-models/user-basic';

@Component({
  selector: 'app-calendar-addcontact-dialog',
  templateUrl: './calendar-addcontact-dialog.component.html',
  styleUrls: ['./calendar-addcontact-dialog.component.css', '../../styles/style.css']
})
export class CalendarAddContactDialogComponent implements OnInit {

  email: string = "";
  //roles = [Role.Member, Role.Administrator];
  //selectedRole: string = "Member";

  constructor(public dialogRef: MatDialogRef<CalendarAddContactDialogComponent>,
    private authService: AuthService) { }

  ngOnInit(): void {
  }

  cancel() {
    this.dialogRef.close("cancel");
  }

  add(){
    this.authService.addContact(this.email).subscribe(result => {
      if (result.Success){
        alert("Contact Added");
        this.dialogRef.close("add");
      }
      else {
        alert(result.Message);
      }
    });
  }
}
