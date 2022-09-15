import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { AuthService } from 'src/app/auth-service/AuthService';
import { DataSharingService } from 'src/app/calendar-service/DataSharingService';
import { GlobalConstants } from 'src/app/common/global-constant';
import { GlobalFuntions } from 'src/app/common/global-functions';
import { MessageboxComponent } from '../messagebox/messagebox.component';

@Component({
  selector: 'app-calendar-changeavatar-dialog',
  templateUrl: './calendar-changeavatar-dialog.component.html',
  styleUrls: ['./calendar-changeavatar-dialog.component.css', '../../styles/style.css']
})
export class CalendarChangeavatarDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<CalendarChangeavatarDialogComponent>
    , private authService: AuthService, private dialog:MatDialog,
     private sanitizer: DomSanitizer, private dataSharingService: DataSharingService) { }

  @ViewChild("file") file : ElementRef;

  profilePic: any;
  uploadIcon = GlobalConstants.uploadIcon;
  removeIcon = GlobalConstants.removeIcon;
  userId: number;

  ngOnInit(): void {
    if (this.authService.getLoggedUserId()){
      this.userId = this.authService.getLoggedUserId();

      this.getAvatar();
    }
  }

  getAvatar(){
    this.authService.getAvatar(this.userId).subscribe(result => {
      this.profilePic = GlobalFuntions.createImageFromBlob(result, this.sanitizer);
    })
  }

  upload(){
    this.file.nativeElement.click(); 
  }

  uploadImage(event: any){
    const fileToUpload = event.target.files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this.authService.uploadProfileImage(formData).subscribe(result => {
      this.getAvatar();
      this.dataSharingService.isProfilePictureChange.next(true);
    });
  }

  remove(){
    let dialogResult = this.dialog.open(MessageboxComponent, {
      width:'500px',
      disableClose: true,
      data: { title: 'Delete', message: 'Are you sure you want to remove profile picture.'}
    })

    dialogResult.afterClosed().subscribe(result => {
      if (result == "ok"){
       this.authService.removeProfilePicture(this.userId).subscribe(result => {
        this.getAvatar();
        this.dataSharingService.isProfilePictureChange.next(true);
       });
      }
    });
  }

  close(){
    this.dialogRef.close();
  }
}