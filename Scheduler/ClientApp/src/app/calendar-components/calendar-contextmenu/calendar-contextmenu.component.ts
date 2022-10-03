import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AppointmentType } from 'src/app/calendar-models/appointmentType-enum';
import { ContextOperationModel } from 'src/app/calendar-models/contextOperation-Model';
import { GlobalConstants } from 'src/app/common/global-constant';

@Component({
  selector: 'app-calendar-contextmenu',
  templateUrl: './calendar-contextmenu.component.html',
  styleUrls: ['./calendar-contextmenu.component.css']
})
export class CalendarContextmenuComponent implements OnInit {

  constructor() { }

  appointmentSmallImg = GlobalConstants.appointmentSmallImageSrc;
  taskSmallImg = GlobalConstants.taskSmallImageSrc;
  reminderSmallImg = GlobalConstants.reminderSmallImageSrc;
  redRemoveIcon = GlobalConstants.redRemoveIcon;
  viewIcon = GlobalConstants.viewIcon;

  @Input() isAdd?: boolean;
  @Input() type?: AppointmentType;
  @Input() isOwner?: boolean;
  @Output() showDialog = new EventEmitter<ContextOperationModel>();

  readonly appointmentTypeList = AppointmentType;

  ngOnInit(): void {
  }

  openDialog(operation: string, type: string){
    let com = new ContextOperationModel();
    com.Operation = operation;
    if (type == "Appointment"){
      com.Type = AppointmentType.Appointment;
    }
    else if (type == "Task"){
      com.Type = AppointmentType.Task;
    }
    else if (type == "Reminder"){
      com.Type = AppointmentType.Reminder;
    }

    this.showDialog.emit(com);
  }
}
