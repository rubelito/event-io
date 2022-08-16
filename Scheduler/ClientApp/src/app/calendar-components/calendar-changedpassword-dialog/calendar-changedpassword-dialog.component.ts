import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth-service/AuthService';
import { PasswordModel } from 'src/app/calendar-models/password-Model';

@Component({
  selector: 'app-calendar-changedpassword-dialog',
  templateUrl: './calendar-changedpassword-dialog.component.html',
  styleUrls: ['./calendar-changedpassword-dialog.component.css', '../../styles/style.css']
})
export class CalendarChangedpasswordDialogComponent implements OnInit {

  oldPassword: string = "";
  newPassword: string = "";
  newRePassword: string = "";

  constructor(public dialogRef: MatDialogRef<CalendarChangedpasswordDialogComponent>,
    private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  close(){
    this.dialogRef.close("cancel");
  }

  changePassword(){
    if (this.newPassword == this.newRePassword){
      let model = new PasswordModel();
      model.OldPassword = this.oldPassword;
      model.NewPassword = this.newPassword;

      this.authService.changePassword(model).subscribe(result => {
        if (result.IsChanged){
          this.authService.logOut().subscribe(result1 => {
            if (result1 == "logout"){
              alert("Password changed success, you will be logout.")
              this.router.navigate(["/"]);
              this.dialogRef.close("");
            }
          });
        }
        else {
          alert(result.Message);
        }
      });
    }
    else {
      alert("The password do not match.");
    }
  }


    
}
