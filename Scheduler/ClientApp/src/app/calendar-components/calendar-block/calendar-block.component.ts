import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, Input, Output, OnInit, EventEmitter} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddEditModel } from 'src/app/calendar-models/addEditModel';
import { Block } from 'src/app/calendar-models/block';
import { ContextMenuValue } from 'src/app/calendar-models/contextMenuValue';
import { DateModel } from 'src/app/calendar-models/dateModel';
import { EventModel } from 'src/app/calendar-models/event-model';
import { RangeType } from 'src/app/calendar-models/rangeType-enum';
import { AppointmentService } from 'src/app/calendar-service/AppointmentService';
import { DataSharingService } from 'src/app/calendar-service/DataSharingService';
import { GlobalConstants } from 'src/app/common/global-constant';
import { MessageboxComponent } from '../messagebox/messagebox.component';

@Component({
  selector: 'app-calendar-block',
  templateUrl: './calendar-block.component.html',
  styleUrls: ['./calendar-block.component.css']
})
export class CalendarBlockComponent implements OnInit {

  @Input() block: Block;
  @Output() showParentContextMenu = new EventEmitter<ContextMenuValue>();
  @Output() refreshList = new EventEmitter();

  readonly rangeType = RangeType;
  repeatIcon = GlobalConstants.repeatIcon;
  showTime: boolean;
  showEventsNow: boolean = false;

  constructor(private dialog:MatDialog, private appointmentService: AppointmentService
    , private dataSharingService: DataSharingService) {
   }
   
  ngOnInit(): void {
    this.dataSharingService.isShowTime.subscribe(change => {
      this.showTime = change;
    })

    this.dataSharingService.eventProcess.subscribe(change => {
      if (this.block.events.length >= 1){
        this.arrangeEvent();
        this.showEventsNow = true;
      }
    });
  }

  arrangeEvent(){
    if (this.block?.previousBlock != undefined){
        var tempEventsBlocks: EventModel[] = [];

      let midsAndEnd = this.block.events.filter((ev) => {
        return ev.RangeType == RangeType.Middle || ev.RangeType == RangeType.End
      });

      let starts = this.block.events.filter((ev) => {
        return ev.RangeType == RangeType.Start
      });

      let noRange = this.block.events.filter((ev) => {
        return ev.RangeType == RangeType.NoRange
      });

      let previosStartAndMids= this.block?.previousBlock.events.filter((ev) => {
        return ev.RangeType == RangeType.Middle || ev.RangeType == RangeType.Start;
      });

      let previousEmptySpaceOnly = this.block?.previousBlock.events.filter((ev) => {
        return ev.IsEmptyEvent;
      });

      if (previosStartAndMids.length >= 1){
        tempEventsBlocks = this.createEmptyEventBlocks(previosStartAndMids.length + previousEmptySpaceOnly.length);

        midsAndEnd.forEach(me => {
          let previousIndex = this.block?.previousBlock.events.findIndex(e => e.DateId == me.DateId);
            tempEventsBlocks[previousIndex] = me;
        });

        starts.forEach(me => {
          tempEventsBlocks.push(me);
        })

        this.block.events = tempEventsBlocks;
        this.removeTrailingEmptyEvents();

        noRange.forEach(rs => {
          this.block.events.push(rs);
        });
      }
    }
  }
  removeTrailingEmptyEvents(){
    for(let i = this.block.events.length - 1; i >= 0; i--){
      if (this.block.events[i].IsEmptyEvent){
        this.block.events.pop();
      }
      else {
        break;
      }
    }
  }
  createEmptyEventBlocks(num: number){
    let tempEvent = [];
    for(let i = 1; i <= num; i++){
      var emptyEvent = new EventModel();
      emptyEvent.Title = "empty";
      emptyEvent.IsEmptyEvent = true;
      tempEvent.push(emptyEvent);
    }

    return tempEvent;
  }

  onRightClickBlock(event: any){
    if (!this.block?.isEmptyBlock){
      let param = new ContextMenuValue();
      param.show = true;
      param.isBlock = true;
      param.isAdd = true;
      param.isOwner = true;
      param.selectedBlock = this.block as Block;
      param.fullScreenLocationX = event.clientX;
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
      param.type = appoint.IsRange ? appoint.MainEventReference.Type : appoint.Type;
      param.isAdd = false;
      param.selectedEvent = appoint.IsRange ? appoint.MainEventReference : appoint;
      param.fullScreenLocationX = event.clientX;
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
    var dateModel = new DateModel();

    if (obj?.classList.contains("real-block")){
      dateModel.Id = appointment.Id;
      dateModel.StrDate = obj.id;

    }
    else if (obj?.classList.contains("block-part")){
      const blockObj = obj.closest('.real-block');

      dateModel.Id = appointment.Id;
      dateModel.StrDate = blockObj!.id;
    }

    if (dateModel != null){
      if (dateModel.StrDate == appointment.Date){
        return;
      }

      let confirmMessage = "Are you sure you want to move this appointment to " + dateModel.StrDate + " ?";
      let dialogResult = this.dialog.open(MessageboxComponent, {
        width:'500px',
            disableClose: true,
            data: { title: 'Move', message: confirmMessage, hasNoCancel: false, icon: "move"}
      });
      
      dialogResult.afterClosed().subscribe(result => {
        if (result == "ok"){
          this.moveEvent(appointment, dateModel);
        }
      });
    }
  }

  moveEvent(appointment: EventModel, dateModel: DateModel) {
    if (appointment.IsClone) {
      var addEditModel = new AddEditModel();
      addEditModel.Appointment = appointment.RangeType != RangeType.NoRange ? appointment.MainEventReference : appointment;
      addEditModel.Appointment.Date = dateModel.StrDate;;
      addEditModel.GroupIds = [];
      addEditModel.MemberIds = [];

      addEditModel.Appointment.MainEventReference = new EventModel();

      this.appointmentService.editRepeat(addEditModel).subscribe(result => {
        if (result == 'Success') {
          this.refreshList.emit();
        }
      });
    }
    else {
      this.appointmentService.changeScheduleDate(dateModel)
        .subscribe(result => {
          if (result == 'Success') {
            this.refreshList.emit();
          }
      });
    }
  }

  getStyleForRangeType(rangeType: RangeType): string {
    let rangeStyle = "";
    if (rangeType == RangeType.NoRange){
      rangeStyle = "norange-event";
    }
    else if (rangeType == RangeType.Start){
      rangeStyle = "start-event";
    }
    else if (rangeType == RangeType.Middle) {
      rangeStyle = "middle-event";
    }
    else if (rangeType == RangeType.End){
      rangeStyle = "end-event";
    }

    return rangeStyle;
  }
}