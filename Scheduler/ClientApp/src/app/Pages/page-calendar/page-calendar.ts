import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { Block } from 'src/app/calendar-models/block';
import { Appointment } from 'src/app/calendar-models/Appointment';
import { ContextMenuValue } from 'src/app/calendar-models/contextMenuValue';
import { DialogOperation } from 'src/app/calendar-models/DialogOperation';
import { AppointmentService } from 'src/app/calendar-service/AppointmentService';
import { AddEditModel } from 'src/app/calendar-models/addEditModel';
import { EventModel } from 'src/app/calendar-models/event-model';
import * as moment from 'moment';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CalendarDialogComponent } from 'src/app/calendar-components/calendar-dialog/calendar-dialog.component';
import { GlobalConstants } from 'src/app/common/global-constant';

@Component({
  selector: 'page-calendar',
  templateUrl: './page-calendar.html',
  styleUrls: ['./page-calendar.css',
  '../../styles/style.css']
})
export class PageCalendarComponent {

  leftArrowIcon = GlobalConstants.leftArrowIcon;
  rightArrowIcon = GlobalConstants.rightArrowIcon;

  title = 'scheduler-app';
  currentMonth: Date = new Date();
  daysInMonth = 0;
  daysBlock: Block[] = [];
  dayToday: number = 0;
  monthToday: number = 0;
  yearToday: number = 0;
  showContextMenu: boolean = false;

  contextMenuX: string = "";
  contextMenuY: string = "";
  contextMenuAdd: any;
  shouldShowDialog: boolean = false;
  dialogParam: DialogOperation = new DialogOperation();
  yearMonth: string = "";

  menuParam: ContextMenuValue = new ContextMenuValue();
  showDeleteDialog: boolean;
  appointmentToDelete: Appointment;
  loggedUser: string = "";

  monthStr: string = "";
  currentYear: number = 0;

  dialogResult: MatDialogRef<any>;

  constructor(private eventService: AppointmentService, private dialog:MatDialog){

  }

  ngOnInit(){
    /* This code is to mark the current day(red mark)  */
    this.dayToday = moment().date();
    this.monthToday = moment().month();
    this.yearToday = moment().year();
    ////////////////////////////////////////
    this.incrementMonth(0);
  }

  incrementMonth(num: number){
    this.currentMonth = new Date(
      this.currentMonth.getFullYear(),
      this.currentMonth.getMonth() + num,
      1);
    
    this.monthStr = moment(this.currentMonth).format("MMMM");
    this.currentYear = this.currentMonth.getFullYear();

    this.daysInMonth = new Date(this.currentMonth.getFullYear(), this.currentMonth.getMonth() + 1, 0).getDate();
    this.yearMonth = this.currentMonth.getMonth() + 1 + "/" + this.currentYear;
    this.generateMonth();
  }

  today(){
    this.shouldShowDialog = false;
    this.currentMonth = new Date();
    this.incrementMonth(0);
  }

  previousMonth(){
    this.shouldShowDialog = false;
    this.incrementMonth(-1);
  }

  nextMonth(){
    this.shouldShowDialog = false;
    this.incrementMonth(1);
  }

  generateMonth(){
    this.daysBlock = [];
    ////Create initial empty block////////
    const date = moment(this.currentMonth);
    const weekRank = date.day();
    
    for (let i = 1; i <= weekRank; i++) {
      let b = new Block();
      b.isEmptyBlock = true;
      this.daysBlock.push(b);
    }

    //////Create Days block
    for (let i = 1; i <= this.daysInMonth; i++) {
      let block = new Block();

      if (this.dayToday == i && this.monthToday == this.currentMonth.getMonth() && this.yearToday == this.currentMonth.getFullYear()){
        block.isToday = true;
      }
      block.day = i;
      block.blockDate = new Date(this.currentMonth.getFullYear(), this.currentMonth.getMonth(), i);
      block.stringDate = moment(this.currentMonth).add(i - 1, "days").format("MM/DD/YYYY");
      this.daysBlock.push(block);
    }

    //Create last empoty block//////

    const lastDateInMonth = moment(new Date(this.currentMonth.getFullYear(), this.currentMonth.getMonth(), this.daysInMonth));
    const weekRankLast = lastDateInMonth.day() + 1;
    const lastBlockNeeded = 7 - weekRankLast;

    for (let i = 1; i <= lastBlockNeeded; i++) {
      let b = new Block();
      b.isEmptyBlock = true;
      this.daysBlock.push(b);
    }

    this.getEvents();
  }

  getEvents(){
    this.eventService.getAppointments(this.yearMonth)
      .subscribe((data: EventModel[]) =>
        {
          this.daysBlock.forEach(b => {
            let tempEvent = data.filter((ev) => {
              return ev.Date == b.stringDate;
            })
            //Sort events by time
            tempEvent.sort(function(a, b) {
              return Date.parse('1970/01/01 ' + a.Time.slice(0, -2) + ' ' + a.Time.slice(-2)) - Date.parse('1970/01/01 ' + b.Time.slice(0, -2) + ' ' + b.Time.slice(-2))
            });
            
            b.events = tempEvent;
          })
        } 
      );
  }

  @HostListener("click", ["$event"])  
  onClick(event: any) {
    if (event.target.localName != "app-calendar-contextmenu"){
      this.showContextMenu = false;
    }
  }

  showThisContextMenu(menu: ContextMenuValue){
    this.menuParam = menu;
    this.showContextMenu = true;
    this.contextMenuX = menu.locationX.toString();
    this.contextMenuY = menu.locationY.toString();
    this.contextMenuAdd = menu.isAdd;
  }

  openDialog(operation: string){
    let param = new DialogOperation();
    param.operation = operation;
    param.appointment = this.menuParam.selectedEvent;
    param.yearMonth = this.yearMonth;

    if (operation == 'Add'){
      this.shouldShowDialog = true;
      param.stringDate = this.menuParam.selectedBlock.stringDate;
    }
    else if (operation == 'Edit'){
      this.shouldShowDialog = true;
      param.stringDate = this.menuParam.selectedEvent.Date;
    }
    else if (operation == 'Delete'){
      this.showDeleteDialog = true;
      this.appointmentToDelete = param.appointment;
    }

    if (operation == "Add" || operation == "Edit"){
      this.showMatDialog(param);
    }
  }

  showMatDialog(dialogParam: DialogOperation){
    this.dialogResult = this.dialog.open(CalendarDialogComponent, {
      width: '800px',
      disableClose: true,
      data: { param : dialogParam}
    });

    this.dialogResult.afterClosed().subscribe(result => {
      if (result != "close"){
        let model = new AddEditModel();
        model.Appointment = result.appointment;
        model.MemberIds = result.membersIds;
        model.GroupIds = result.groupIds;
    
        if (result.operation == 'Add'){
          this.eventService.addSchedule(model)
            .subscribe(returnData => {
              console.log(returnData);
              this.shouldShowDialog = false;
              this.generateMonth();
            });
        }
        else if (result.operation == 'Edit'){
          this.eventService.editSchedule(model)
            .subscribe(returnData => {
                console.log(returnData);
                this.shouldShowDialog = false;
                this.generateMonth();
              }
            );
        }
      }
    });
  }

  onOkDelete(){
    this.eventService.deleteSchedule(this.appointmentToDelete.Id).subscribe(returnData => {
      console.log(returnData);
      this.showDeleteDialog = false;
      this.generateMonth();
    });
  }

  onDeleteCancel(){
    this.showDeleteDialog = false;
  }

  refreshEvents(){
    this.generateMonth();
  }
}