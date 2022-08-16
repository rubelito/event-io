import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth-service/AuthService';
import { CalendarChangedpasswordDialogComponent } from 'src/app/calendar-components/calendar-changedpassword-dialog/calendar-changedpassword-dialog.component';
import { User } from 'src/app/calendar-models/user';
import { UserBasic } from 'src/app/calendar-models/user-basic';

@Component({
  selector: 'app-page-profile',
  templateUrl: './page-profile.component.html',
  styleUrls: ['./page-profile.component.css']
})
export class PageProfileComponent implements OnInit {

  user: User;
  
  firstName: string = "";
  middleName: string = "";
  lastName: string = "";

  preFirstName: string = "";
  preMiddleName: string = "";
  preLastName: string = "";

  constructor(private authService: AuthService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(data => {
      this.user = data;

      this.firstName = data.FirstName;
      this.middleName = data.MiddleName;
      this.lastName = data.LastName;

      this.preFirstName = data.FirstName;
      this.preMiddleName = data.MiddleName;
      this.preLastName = data.LastName;
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

  cancel(){
    this.firstName = this.preFirstName;
    this.middleName = this.preMiddleName;
    this.lastName = this.preLastName;
  }

  changePass(){
    this.dialog.open(CalendarChangedpasswordDialogComponent, {
      width: '250px'
    });
  }
}
