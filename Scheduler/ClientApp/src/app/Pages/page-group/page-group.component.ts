import { Component, HostListener, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CalendarAddeditGroupDialogComponent } from 'src/app/calendar-components/calendar-addedit-group-dialog/calendar-addedit-group-dialog.component';
import { CalendarAddremoveMemberDialogComponent } from 'src/app/calendar-components/calendar-addremove-member-dialog/calendar-addremove-member-dialog.component';
import { GroupResultModel } from 'src/app/calendar-models/groupResult-Model';
import { GroupService } from 'src/app/calendar-service/GroupService';

@Component({
  selector: 'app-page-group',
  templateUrl: './page-group.component.html',
  styleUrls: ['./page-group.component.css', '../../styles/style.css']
})
export class PageGroupComponent implements OnInit {
  constructor(private groupService: GroupService, private dialog:MatDialog) { }

  displayedColumns: string[] = ['id', 'name', 'owner', 'owneremail', 'members'];
  groups: GroupResultModel[] = [];

  ngOnInit(): void {
    this.loadGroups();
  }
  
  loadGroups(){
    this.groupService.getYourGroupListWithMembers().subscribe(results => {
      this.groups = results;
    });
  }

  openMembersDialog(groupId: number){
    let dialogResult = this.dialog.open(CalendarAddremoveMemberDialogComponent, {
      disableClose: true,
      data: groupId
    });

    dialogResult.afterClosed().subscribe(result => {
      if (result == 'saved'){
        this.loadGroups();
      }
    });
  }

  openAddDialog(){
    let dialogResult = this.dialog.open(CalendarAddeditGroupDialogComponent, {
      width: "800px",
      disableClose: true
    });

    dialogResult.afterClosed().subscribe(result => {
      if (result == "add"){
        this.loadGroups();
      }
    });
  }
}