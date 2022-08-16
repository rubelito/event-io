import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth-service/AuthService';
import { UserBasic } from 'src/app/calendar-models/user-basic';
import { GlobalConstants } from 'src/app/common/global-constant';
import { CalendarLoginDialogComponent } from '../calendar-login-dialog/calendar-login-dialog.component';

@Component({
  selector: 'app-calendar-register-dialog',
  templateUrl: './calendar-register-dialog.component.html',
  styleUrls: ['./calendar-register-dialog.component.css']
})
export class CalendarRegisterDialogComponent implements OnInit {
  @ViewChild('usernameRef') private usernameRef: ElementRef;
  @ViewChild('emailRef') private emailRef: ElementRef;

  @ViewChild('password1Ref') private password1Ref: ElementRef;
  @ViewChild('password2Ref') private password2Ref: ElementRef;

  showPasswordIcon = GlobalConstants.showPasswordIcon;
  hidePasswordIcon = GlobalConstants.hidePasswordIcon;

  username: string = ''; 
  email: string = '';
  password1: string = '';
  password2: string = '';
  lastname: string = '';
  firstname: string = '';
  middlename: string = '';

  isUsernameExist: boolean;
  isEmailExist: boolean;

  show1: boolean = false;
  show2: boolean = false;

  constructor(public dialogRef: MatDialogRef<CalendarRegisterDialogComponent>,
     private authService: AuthService, private loginDialogRef: MatDialog) { }

  ngOnInit(): void {
  }

  showPassword1(){
    this.show1 = !this.show1;
    this.password1Ref.nativeElement.focus();
  }

  showPassword2(){
    this.show2 = !this.show2;
    this.password2Ref.nativeElement.focus();
  }

  onCancel(){
    this.dialogRef.close();
  }

  onRegister(){
    if (this.password1 != this.password2){
      alert("Password does not match");
      return;
    }

    let newUser = new UserBasic();

    newUser.UserName = this.username;
    newUser.FirstName = this.firstname;
    newUser.LastName = this.lastname;
    newUser.MiddleName = this.middlename;
    newUser.Email = this.email;
    newUser.Password = this.password1;
    newUser.Enabled = true;

    this.authService.isUsernameExist(newUser.UserName).subscribe(data => {
      this.isUsernameExist = data;
      if (!this.isUsernameExist){
        this.authService.isEmailExist(newUser.Email).subscribe(data => {
          this.isEmailExist = data;
          if (!this.isEmailExist){
            this.authService.register(newUser).subscribe(data => {
              if (data.Success){
                alert("Register successful, please login using your Username and Password");
                this.dialogRef.close();
                let loginDialog = this.loginDialogRef.open(CalendarLoginDialogComponent, {
                  width: '800px',
                  disableClose: true
                });
              }
              else {
                alert(data.Message);
              }
            })
          }
          else {
            this.emailRef.nativeElement.select();
            this.emailRef.nativeElement.focus();
            alert("Email already used");
          }
        });
      }
      else {
        this.usernameRef.nativeElement.select();
        this.usernameRef.nativeElement.focus();
        alert("Username already used");
      }
    });
  }
}
