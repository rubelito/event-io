import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { Block } from 'src/app/calendar-models/block';
import { ContextMenuValue } from 'src/app/calendar-models/contextMenuValue';
import { DateModel } from 'src/app/calendar-models/dateModel';
import { EventModel } from 'src/app/calendar-models/event-model';
import { AppointmentService } from 'src/app/calendar-service/AppointmentService';

@Component({
  selector: 'app-calendar-block',
  templateUrl: './calendar-block.component.html',
  styleUrls: ['./calendar-block.component.css']
})
export class CalendarBlockComponent implements OnInit {

  @Input() block?: Block;
  @Output() showParentContextMenu = new EventEmitter<ContextMenuValue>();
  @Output() refreshList = new EventEmitter();

  constructor(private appointmentService: AppointmentService) {
   }
  ngOnInit(): void {
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
      param.isAdd = false;
      param.selectedEvent = appoint;
      param.locationX = event.layerX;
      param.locationY = event.layerY;

    this.showParentContextMenu.emit(param);
    }
  }

  getEventBGColor(isOwner: boolean){
    let color = "#3f8b3f";
    if (!isOwner){
      color = "#475386";
    }

    return color;
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
    var dateModel = null;

    if (obj?.classList.contains("real-block")){
      dateModel = new DateModel();
      dateModel.Id = (e.item.data as EventModel).Id;
      dateModel.StrDate = obj.id;

    }
    else if (obj?.classList.contains("block-part")){
      const blockObj = obj.closest('.real-block');

      dateModel = new DateModel();
      dateModel.Id = (e.item.data as EventModel).Id;
      dateModel.StrDate = blockObj!.id;
    }

    if (dateModel != null){
      if (dateModel.StrDate == (e.item.data as EventModel).Date){
        return;
      }

      let confirmMessage = "Are you sure you want to move this appointment to " + dateModel.StrDate + " ?";
      if (confirm(confirmMessage)){
        this.appointmentService.changeScheduleDate(dateModel)
          .subscribe(data => {
            if (data == 'Success'){
              this.refreshList.emit();
            }
          });
      }
    }
  }
}
