import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth-service/AuthService';
import { CalendarLoginDialogComponent } from '../calendar-login-dialog/calendar-login-dialog.component';

@Component({
  selector: 'app-calendar-logout-dialog',
  templateUrl: './calendar-logout-dialog.component.html',
  styleUrls: ['./calendar-logout-dialog.component.css', '../../styles/style.css']
})
export class CalendarLogoutDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CalendarLoginDialogComponent>, private router: Router, private auth: AuthService) { }

  ngOnInit(): void {
  }

  onLogout(){
    this.auth.logOut().subscribe(result => {
      if (result == 'logout'){
        this.dialogRef.close();
        this.router.navigate(["/"]);
      }
    })
  }

  onCancel(){
    this.dialogRef.close();
  }
}