import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CalendarAddeditGroupDialogComponent } from 'src/app/calendar-components/calendar-addedit-group-dialog/calendar-addedit-group-dialog.component';
import { CalendarAddremoveMemberDialogComponent } from 'src/app/calendar-components/calendar-addremove-member-dialog/calendar-addremove-member-dialog.component';
import { GroupResultModel } from 'src/app/calendar-models/groupResult-Model';
import { GroupService } from 'src/app/calendar-service/GroupService';
import { GlobalConstants } from 'src/app/common/global-constant';
import { DataSharingService } from 'src/app/calendar-service/DataSharingService';

@Component({
  selector: 'app-page-group',
  templateUrl: './page-group.component.html',
  styleUrls: ['./page-group.component.css', '../../styles/style.css']
})
export class PageGroupComponent implements OnInit, AfterViewInit {
  constructor(private groupService: GroupService,
    private dialog:MatDialog,
    private dataSharingService: DataSharingService) { }

  createIcon = GlobalConstants.createIcon;
  displayedColumns: string[] = ['id', 'name', 'owner', 'owneremail', 'members'];
  groups: GroupResultModel[] = [];
  showLargeTable: boolean = true;

  ngOnInit(): void {
    this.loadGroups();
    this.responsiveness();
    
  }

  private responsiveness(){
    this.dataSharingService.tableSize.subscribe(res => {
      this.showLargeTable = res;
    });
  }
  
  loadGroups(){
    this.groupService.getYourGroupListWithMembers().subscribe(results => {
      this.groups = results;
    });
  }

  ngAfterViewInit(): void {
    this.dataSharingService.toggleMenu.next();
  }

  openMembersDialog(groupId: number){
    let dialogResult = this.dialog.open(CalendarAddremoveMemberDialogComponent, {
      width: "500px",
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
      width: "500px",
      disableClose: true
    });

    dialogResult.afterClosed().subscribe(result => {
      if (result == "add"){
        this.loadGroups();
      }
    });
  }
}