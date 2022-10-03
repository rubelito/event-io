import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth-service/AuthService';
import { UserCredential } from 'src/app/calendar-models/user-credential';
import { DataSharingService } from 'src/app/calendar-service/DataSharingService';
import { GlobalConstants } from 'src/app/common/global-constant';
import { MessageboxComponent } from '../messagebox/messagebox.component';

@Component({
  selector: 'app-calendar-login-dialog',
  templateUrl: './calendar-login-dialog.component.html',
  styleUrls: ['./calendar-login-dialog.component.css']
})
export class CalendarLoginDialogComponent implements OnInit {
  @ViewChild('password1Ref') private password1Ref: ElementRef;
  textHeader: string = '';
  userName: string ='';
  password: string = '';

  showPasswordIcon = GlobalConstants.showPasswordIcon;
  hidePasswordIcon = GlobalConstants.hidePasswordIcon;
  lockIcon = GlobalConstants.lockIcon;

  show: boolean = false;

  constructor(public dialogRef: MatDialogRef<CalendarLoginDialogComponent>, 
    private authService: AuthService, private router: Router, 
    private dialog:MatDialog, private dataSharingService: DataSharingService) { }

  ngOnInit(): void {
  }

  showPassword(){
    this.show = !this.show;
    this.password1Ref.nativeElement.focus();
    
  }

  onAuth(){
    let userCredential = new UserCredential();
    userCredential.Username = this.userName;
    userCredential.Password = this.password;
    this.authService.logIn(userCredential).subscribe(data => {
      if (data.IsAuthenticated){
        localStorage.setItem("id", data.Id.toString());
        localStorage.setItem("username", this.userName);
        localStorage.setItem("isLogin", data.IsAuthenticated.toString());
        localStorage.setItem("loginStatus", data.LoginStatus);
        localStorage.setItem('accessToken', "basic " + data.Credential);
        localStorage.setItem('role', data.Role);

        this.dataSharingService.isProfilePictureChange.next(true);
        this.dialogRef.close();
        this.router.navigate(["/Schedules"]);
      }
      else {
        this.showMessage("Invalid Username and Password", "warning");
      }
    });
  }

  onRegisterNow(){
    this.dialogRef.close("onRegisterNow");
  }

  onCancel(){
    this.dialogRef.close();
  }

  showMessage(message: string, icon: string){
    this.dialog.open(MessageboxComponent, {
      width:'400px',
      disableClose: true,
      data: { title: '', message: message, hasNoCancel: true, icon: icon}
    });
  }
}
