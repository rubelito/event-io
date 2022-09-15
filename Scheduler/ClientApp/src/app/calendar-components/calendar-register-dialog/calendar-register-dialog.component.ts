import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth-service/AuthService';
import { UserBasic } from 'src/app/calendar-models/user-basic';
import { GlobalConstants } from 'src/app/common/global-constant';
import { CalendarLoginDialogComponent } from '../calendar-login-dialog/calendar-login-dialog.component';
import { MessageboxComponent } from '../messagebox/messagebox.component';

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
     private authService: AuthService, private dialog:MatDialog) { }

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
      this.dialog.open(MessageboxComponent, {
        width:'400px',
        disableClose: true,
        data: { title: '', message: "Password does not match", hasNoCancel: true, icon: "warning"}
      });
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
                let dialogResult = this.dialog.open(MessageboxComponent, {
                  width:'400px',
                  disableClose: true,
                  data: { title: '', message: "Register successful, please login using your Username and Password", hasNoCancel: true, icon: "ok"}
                });
                dialogResult.afterClosed().subscribe(result => {
                  this.dialogRef.close();
                  this.dialog.open(CalendarLoginDialogComponent, {
                  width: '800px',
                  disableClose: true
                });
              });
              }
              else {
                this.showMessage(data.Message, "x");
              }
            })
          }
          else {
            this.emailRef.nativeElement.select();
            this.emailRef.nativeElement.focus();
            this.showMessage("Email already used", "warning");
          }
        });
      }
      else {
        this.usernameRef.nativeElement.select();
        this.usernameRef.nativeElement.focus();
        this.showMessage("Username already used", "warning");
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
