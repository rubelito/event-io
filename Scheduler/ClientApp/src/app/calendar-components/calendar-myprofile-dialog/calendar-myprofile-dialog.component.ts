import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { User } from 'src/app/calendar-models/user';
import { AuthService } from 'src/app/auth-service/AuthService';
import { CalendarLoginDialogComponent } from '../calendar-login-dialog/calendar-login-dialog.component';
import { UserBasic } from 'src/app/calendar-models/user-basic';
import { CalendarChangedpasswordDialogComponent } from '../calendar-changedpassword-dialog/calendar-changedpassword-dialog.component';
import { GlobalFuntions } from 'src/app/common/global-functions';
import { DomSanitizer } from '@angular/platform-browser';
import { CalendarChangeavatarDialogComponent } from '../calendar-changeavatar-dialog/calendar-changeavatar-dialog.component';
import { DataSharingService } from 'src/app/calendar-service/DataSharingService';

@Component({
  selector: 'app-calendar-myprofile-dialog',
  templateUrl: './calendar-myprofile-dialog.component.html',
  styleUrls: ['./calendar-myprofile-dialog.component.css', '../../styles/style.css']
})
export class CalendarMyprofileDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CalendarLoginDialogComponent>,
    private authService: AuthService, 
    private dialog: MatDialog,
    private sanitizer: DomSanitizer, 
    private dataSharingService: DataSharingService) { }

  user: User;

  firstName: string = "";
  middleName: string = "";
  lastName: string = "";

  preFirstName: string = "";
  preMiddleName: string = "";
  preLastName: string = "";

  profilePic: any;

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(data => {
      this.user = data;
      
      this.firstName = data.FirstName;
      this.middleName = data.MiddleName;
      this.lastName = data.LastName;

      this.preFirstName = data.FirstName;
      this.preMiddleName = data.MiddleName;
      this.preLastName = data.LastName;

      this.dataSharingService.isProfilePictureChange.subscribe(res => {
        this.loadProfilePic();
      })
    })
  }

  loadProfilePic(){
    this.authService.getAvatar(this.user.Id).subscribe(result => {
      this.profilePic = GlobalFuntions.createImageFromBlob(result, this.sanitizer);
    })
  }

  onUpdate(){
    let uUser = new UserBasic();

    uUser.FirstName = this.firstName;
    uUser.MiddleName = this.middleName;
    uUser.LastName = this.lastName;
    this.authService.editCurrentUser(uUser).subscribe(data => {
      this.firstName = data.FirstName;
      this.middleName = data.MiddleName;
      this.lastName = data.LastName;

      this.preFirstName = data.FirstName;
      this.preMiddleName = data.MiddleName;
      this.preLastName = data.LastName;
    });
  }

  reset(){
    this.firstName = this.preFirstName;
    this.middleName = this.preMiddleName;
    this.lastName = this.preLastName;
  }

  changePass(){
    let dialogResult = this.dialog.open(CalendarChangedpasswordDialogComponent, {
      width: '400px',
      disableClose: true
    });

    dialogResult.afterClosed().subscribe(result => {
      if (result == 'changed'){
        this.dialogRef.close();
      }
    });
  }

  avatarClick(){
    let dialogResult = this.dialog.open(CalendarChangeavatarDialogComponent, {
      width: '400px',
      disableClose: true
    });

    dialogResult.afterClosed().subscribe(result => {
      if (result == 'changed'){
        this.dialogRef.close();
      }
    });
  }

  close(){
    this.dialogRef.close();
  }
}
