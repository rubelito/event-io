import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { UserBasic } from 'src/app/calendar-models/user-basic';
import { GroupService } from 'src/app/calendar-service/GroupService';
import { AuthService } from 'src/app/auth-service/AuthService';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MembersOfGroupModel } from 'src/app/calendar-models/membersOfGroup-model';
import { MessageboxComponent } from '../messagebox/messagebox.component';
import { GlobalConstants } from 'src/app/common/global-constant';

@Component({
  selector: 'app-calendar-addremove-member-dialog',
  templateUrl: './calendar-addremove-member-dialog.component.html',
  styleUrls: ['./calendar-addremove-member-dialog.component.css', '../../styles/style.css']
})
export class CalendarAddremoveMemberDialogComponent implements OnInit {

  constructor(private groupService: GroupService, private authService: AuthService,
    @Inject(MAT_DIALOG_DATA) public data: number, public dialogRef: MatDialogRef<CalendarAddremoveMemberDialogComponent>
    , private dialog:MatDialog) { }
  members: UserBasic[] = [];
  users: UserBasic[] = [];

  selectedMember: UserBasic;
  selectedUser: UserBasic;

  clonedUsers: UserBasic[] = [];

  status: string = "no-changes";

  leftArrowIcon = GlobalConstants.leftArrowIcon;
  rightArrowIcon = GlobalConstants.rightArrowIcon;

  leftOrRight: string;

  ngOnInit(): void {
    this.loadMembersAndUser();
  }

  loadMembersAndUser(){
    this.groupService.getUsersInGroup(this.data).subscribe(result => {
      this.members = result;
      this.clonedUsers = JSON.parse(JSON.stringify(result));
    });

    this.authService.getAllActiveUser().subscribe(result => {
      this.users = result;
    })
  }

  addMember(){
    if (this.selectedUser != null){
      let doesExist = this.members.some(u => u.Id == this.selectedUser.Id);

      if (doesExist){
        this.dialog.open(MessageboxComponent, {
          width:'400px',
          disableClose: true,
          data: { title: '', message: "User is already a member", hasNoCancel: true, icon: "warning"}
        });
        return;
      }

      this.members.push(this.selectedUser);
    }
  }

  removeMember(){
    this.members =this.members.filter(im => {
      return im.Id != this.selectedMember.Id;
    })
  }

  save(){
    ///Determine the users to be added on the list
    var addModels = new MembersOfGroupModel();
    addModels.GroupId = this.data;

    var membersToBeAdded: UserBasic[] = [];
    this.members.forEach(m => {
      var exist = this.clonedUsers.some(c => c.Id == m.Id);
      if (!exist){
        membersToBeAdded.push(m);
      }
    });
    addModels.Members = membersToBeAdded.map(m => m.Id);

    ///Determine the user to be remove on the list
    var removeModels = new MembersOfGroupModel();
    removeModels.GroupId = this.data;

    var membersToBeRemove: UserBasic[] = [];
    this.clonedUsers.forEach(c => {
      var exist = this.members.some(m => c.Id == m.Id);
      if (!exist){
        membersToBeRemove.push(c);
      }
    });
    removeModels.Members = membersToBeRemove.map(m => m.Id);

    this.groupService.addMembersToGroup(addModels).subscribe(result => {
      if (result.Success){
        this.groupService.removeMembersToGroup(removeModels).subscribe(result1 => {
          if (result1.Success){
            this.dialogRef.close("saved");
          }
        })
      }
    });
  }

  cancel(){
    this.dialogRef.close("cancel");
  }

  focusedSelect(leftRightSelected: string){
    this.leftOrRight = leftRightSelected;
  }
}