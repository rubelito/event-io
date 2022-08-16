import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth-service/AuthService';
import { UserBasic } from 'src/app/calendar-models/user-basic';
import { UserCredential } from 'src/app/calendar-models/user-credential';

@Component({
  selector: 'app-page-login',
  templateUrl: './page-login.component.html',
  styleUrls: ['./page-login.component.css']
})
export class PageLoginComponent implements OnInit {

  @ViewChild('usernameRef') private usernameRef: ElementRef;
  @ViewChild('emailRef') private emailRef: ElementRef;

  mode: string;
  userNameLogin: string ='';
  passwordLogin: string = '';

  usernameReg: string = '';
  password1Reg: string = '';
  password2Reg: string = '';
  emailReg: string = '';
  lastnameReg: string = '';
  firstnameReg: string = '';
  middlenameReg: string = '';

  isUsernameExist: boolean;
  isEmailExist: boolean;

  constructor(public authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.mode = "login";
  }

  onSelectMode(mode: string){
    this.mode = mode;
  }

  register(){

    if (this.password1Reg != this.password2Reg){
      alert("Password does not match");
      return;
    }

    let newUser = new UserBasic();

    newUser.UserName = this.usernameReg;
    newUser.FirstName = this.firstnameReg;
    newUser.LastName = this.lastnameReg;
    newUser.MiddleName = this.middlenameReg;
    newUser.Email = this.emailReg;
    newUser.Password = this.password1Reg;
    newUser.Enabled = true;

    this.authService.isUsernameExist(newUser.UserName).subscribe(data => {
      this.isUsernameExist = data;
      if (!this.isUsernameExist){
        this.authService.isEmailExist(newUser.Email).subscribe(data => {
          this.isEmailExist = data;
          if (!this.isEmailExist){
            this.authService.register(newUser).subscribe(data => {
              alert("Register successful, please login using your Username and Password");
              this.mode = 'login';
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

  login(){
    let userCredential = new UserCredential();
    userCredential.Username = this.userNameLogin;
    userCredential.Password = this.passwordLogin;
    this.authService.logIn(userCredential).subscribe(data => {
      if (data.IsAuthenticated){
        localStorage.setItem("username", this.userNameLogin);
        localStorage.setItem("isLogin", data.IsAuthenticated.toString());
        localStorage.setItem("loginStatus", data.LoginStatus);
        localStorage.setItem('accessToken', "basic " + data.Credential);
        localStorage.setItem('role', data.Role);

        this.router.navigate(["/Schedules"]);
      }
      else {
        alert("Invalid Username and Password");
      }
    });
  }
}