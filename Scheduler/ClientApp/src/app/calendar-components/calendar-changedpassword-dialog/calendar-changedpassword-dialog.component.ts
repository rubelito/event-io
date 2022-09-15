import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth-service/AuthService';
import { PasswordModel } from 'src/app/calendar-models/password-Model';
import { MessageboxComponent } from '../messagebox/messagebox.component';

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
    private authService: AuthService, private router: Router
    , private dialog:MatDialog) { }

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
              let dialogResult = this.dialog.open(MessageboxComponent, {
                width:'400px',
                disableClose: true,
                data: { title: '', message: "Password changed success, you will be logout.", hasNoCancel: true, icon: "ok"}
              });

              dialogResult.afterClosed().subscribe(result => {
                this.router.navigate(["/"]);
                this.dialogRef.close("changed");
              });
            }
          });
        }
        else {
          this.showMessage(result.Message, "warning");
        }
      });
    }
    else {
      this.showMessage("The password do not match.", "warning");
    }
  }

  showMessage(message: string, icon: string){
    this.dialog.open(MessageboxComponent, {
      width:'400px',
      disableClose: true,
      data: { title: '', message: message, hasNoCancel: true, icon: icon}
    });
  }
}
