import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { CreateEditGroupModel } from 'src/app/calendar-models/createEditGroupModel';
import { GroupService } from 'src/app/calendar-service/GroupService';

@Component({
  selector: 'app-calendar-addedit-group-dialog',
  templateUrl: './calendar-addedit-group-dialog.component.html',
  styleUrls: ['./calendar-addedit-group-dialog.component.css', '../../styles/style.css']
})
export class CalendarAddeditGroupDialogComponent implements OnInit {
  id: number = 0;
  groupName: string = "";
  description: string = "";

  constructor(private groupService: GroupService, public dialogRef: MatDialogRef<CalendarAddeditGroupDialogComponent>) { }

  ngOnInit(): void {
  }

  add(){
    let newGroup = new CreateEditGroupModel()
    newGroup.GroupName = this.groupName;
    newGroup.Description = this.description;

    this.groupService.addGroup(newGroup).subscribe(result => {
      if (result.Success){
        this.dialogRef.close("add");
      }
    });
  }

  cancel(){
    this.dialogRef.close("cancel");
  }
}