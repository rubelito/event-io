import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AttendeeModel, AttendeeType } from 'src/app/calendar-models/AttendeeModel';
import { DialogOperation } from 'src/app/calendar-models/DialogOperation';
import { AppointmentService } from 'src/app/calendar-service/AppointmentService';
import { GroupService } from 'src/app/calendar-service/GroupService';
import { GlobalConstants } from 'src/app/common/global-constant';
import { CalendarDialogComponent } from '../calendar-dialog/calendar-dialog.component';

@Component({
  selector: 'app-calendar-view-dialog',
  templateUrl: './calendar-view-dialog.component.html',
  styleUrls: ['./calendar-view-dialog.component.css']
})
export class CalendarViewDialogComponent implements OnInit {

  personImageSrc: string = GlobalConstants.personImageSrc;
  groupImageSrc: string = GlobalConstants.groupImageSrc;

  event: any;
  attendees: Array<AttendeeModel> = []; 

  constructor(public dialogRef: MatDialogRef<CalendarDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {param: DialogOperation},
    private appointmentService: AppointmentService, private groupService: GroupService) { }

  ngOnInit(): void {
    this.event = this.data.param;

    this.appointmentService.getAllAttendees(this.event.appointment.Id).subscribe(data => {
      data.forEach(d => {
        var am = new AttendeeModel();
        am.Id = d.Id;
        am.Name = d.FirstName + " " + d.LastName;
        am.Type = AttendeeType.Member;

        this.attendees.push(am);
      });
    });

    this.groupService.getAllGroupAttendees(this.event.appointment.Id).subscribe(data => {
      data.forEach(d => {
        var am = new AttendeeModel();
        am.Id = d.Id;
        am.Name = d.Name;
        am.Type = AttendeeType.Group;

        this.attendees.push(am);
      });
    });
  }

  close(){
    this.dialogRef.close('close');
  }
}
