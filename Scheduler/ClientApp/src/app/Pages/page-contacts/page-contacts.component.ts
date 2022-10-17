import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/auth-service/AuthService';
import { CalendarAddContactDialogComponent } from 'src/app/calendar-components/calendar-addcontact-dialog/calendar-addcontact-dialog.component';
import { MessageboxComponent } from 'src/app/calendar-components/messagebox/messagebox.component';
import { UserBasic } from 'src/app/calendar-models/user-basic';
import { GlobalConstants } from 'src/app/common/global-constant';

@Component({
  selector: 'app-page-contacts',
  templateUrl: './page-contacts.component.html',
  styleUrls: ['./page-contacts.component.css', '../../styles/style.css']
})
export class PageContactsComponent implements OnInit {
  addIcon = GlobalConstants.addIcon;
  removeIcon = GlobalConstants.removeIcon;
  users: UserBasic[] = [];
  usersToUpdate: UserBasic[] = [];
  displayedColumns: string[] = ['id', 'username', 'name', 'email', 'delete'];
  dialogResult: MatDialogRef<any>;

  constructor(private authService: AuthService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(){
    this.usersToUpdate = [];
    this.authService.getContacts().subscribe(data => {
      this.users = data;
    });
  }

  remove(contactId: number, email: string){
    this.dialogResult = this.dialog.open(MessageboxComponent, {
        width: '400px',
        disableClose: false,
        data: {title: "Delete", message: "Are you sure you want to remove '" + email + "' on your contacts?"} 
    });

    this.dialogResult.afterClosed().subscribe(res => {
      if (res == "ok"){
        this.authService.removeContact(contactId).subscribe(result => {
          this.loadUsers();
        })
      }
    });
  }

  openAddDialog(){
    let dialogResult = this.dialog.open(CalendarAddContactDialogComponent, {
      width: "500px",
      disableClose: true
    });

    dialogResult.afterClosed().subscribe(result => {
      if (result == "add"){
        this.loadUsers();
      }
    });
  }
}