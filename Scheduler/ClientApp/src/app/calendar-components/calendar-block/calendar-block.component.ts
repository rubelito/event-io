import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { AddEditModel } from 'src/app/calendar-models/addEditModel';
import { Block } from 'src/app/calendar-models/block';
import { ContextMenuValue } from 'src/app/calendar-models/contextMenuValue';
import { DateModel } from 'src/app/calendar-models/dateModel';
import { EventModel } from 'src/app/calendar-models/event-model';
import { AppointmentService } from 'src/app/calendar-service/AppointmentService';
import { DataSharingService } from 'src/app/calendar-service/DataSharingService';
import { GlobalConstants } from 'src/app/common/global-constant';

@Component({
  selector: 'app-calendar-block',
  templateUrl: './calendar-block.component.html',
  styleUrls: ['./calendar-block.component.css']
})
export class CalendarBlockComponent implements OnInit {

  @Input() block?: Block;
  @Output() showParentContextMenu = new EventEmitter<ContextMenuValue>();
  @Output() refreshList = new EventEmitter();

  repeatIcon = GlobalConstants.repeatIcon;
  showTime: boolean;

  constructor(private appointmentService: AppointmentService
    , private dataSharingService: DataSharingService) {
   }
  ngOnInit(): void {
    this.dataSharingService.isShowTime.subscribe(change => {
      this.showTime = change;
    })
  }

  onRightClickBlock(event: any){
    if (!this.block?.isEmptyBlock){
      let param = new ContextMenuValue();
      param.show = true;
      param.isBlock = true;
      param.isAdd = true;
      param.isOwner = true;
      param.selectedBlock = this.block as Block;
      param.locationX = event.layerX;
      param.locationY = event.layerY;
      this.showParentContextMenu.emit(param);
    }
  }

  onRightClickAppointment(appoint: EventModel, event: any){
    if (!this.block?.isEmptyBlock){
      let param = new ContextMenuValue();
      param.show = true;
      param.isBlock = false;
      param.isOwner = appoint.IsOwner;
      param.type = appoint.Type;
      param.isAdd = false;
      param.selectedEvent = appoint;
      param.locationX = event.layerX;
      param.locationY = event.layerY;
    this.showParentContextMenu.emit(param);
    }
  }

  getBGColor(){
    let color = "transparent"
    if (!this.block?.isEmptyBlock){
      color = "#272829";
    }

    return color;
  }

  drop(e: CdkDragDrop<string[]>){
    let obj = document.elementFromPoint(e.dropPoint.x, e.dropPoint.y);
    var appointment = e.item.data as EventModel;
    var dateModel = null;


    if (obj?.classList.contains("real-block")){
      dateModel = new DateModel();
      dateModel.Id = appointment.Id;
      dateModel.StrDate = obj.id;

    }
    else if (obj?.classList.contains("block-part")){
      const blockObj = obj.closest('.real-block');

      dateModel = new DateModel();
      dateModel.Id = appointment.Id;
      dateModel.StrDate = blockObj!.id;
    }

    if (dateModel != null){
      if (dateModel.StrDate == appointment.Date){
        return;
      }

      let confirmMessage = "Are you sure you want to move this appointment to " + dateModel.StrDate + " ?";
      if (confirm(confirmMessage)){
        if (appointment.IsClone){
          var addEditModel = new AddEditModel();
          appointment.Date = dateModel.StrDate;
          addEditModel.Appointment = appointment;
          addEditModel.GroupIds = [];
          addEditModel.MemberIds = [];

          this.appointmentService.editRepeat(addEditModel).subscribe(result => {
            if (result == 'Success'){
              this.refreshList.emit();
            }
          });
        }
        else {
          this.appointmentService.changeScheduleDate(dateModel)
          .subscribe(result => {
            if (result == 'Success'){
              this.refreshList.emit();
            }
          });
        }
      }
    }
  }
}