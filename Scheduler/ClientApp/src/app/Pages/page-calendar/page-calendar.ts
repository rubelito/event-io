import { Component, HostListener } from '@angular/core';
import { Block } from 'src/app/calendar-models/block';
import { ContextMenuValue } from 'src/app/calendar-models/contextMenuValue';
import { DialogOperation } from 'src/app/calendar-models/DialogOperation';
import { AppointmentService } from 'src/app/calendar-service/AppointmentService';
import { EventModel } from 'src/app/calendar-models/event-model';
import * as moment from 'moment';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CalendarDialogComponent } from 'src/app/calendar-components/calendar-dialog/calendar-dialog.component';
import { GlobalConstants } from 'src/app/common/global-constant';
import { CalendarViewDialogComponent } from 'src/app/calendar-components/calendar-view-dialog/calendar-view-dialog.component';
import { MessageboxComponent } from 'src/app/calendar-components/messagebox/messagebox.component';
import { ContextOperationModel } from 'src/app/calendar-models/contextOperation-Model';
import { AppointmentType } from 'src/app/calendar-models/appointmentType-enum';

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
  contextMenuType: AppointmentType;
  contextMenuOwner: any;
  shouldShowDialog: boolean = false;
  dialogParam: DialogOperation = new DialogOperation();
  yearMonth: string = "";

  menuParam: ContextMenuValue = new ContextMenuValue();
  appointmentToDelete: EventModel;
  loggedUser: string = "";

  monthStr: string = "";
  currentYear: number = 0;

  dialogResult: MatDialogRef<any>;

  constructor(private eventService: AppointmentService,
     private dialog:MatDialog){
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
    this.refreshEvents();
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
    this.contextMenuType = menu.type;
    this.contextMenuOwner = menu.isOwner;
  }

  openDialog(operation: ContextOperationModel){
    let param = new DialogOperation();
    param.contextOperation = operation;
    param.appointment = this.menuParam.selectedEvent;
    param.yearMonth = this.yearMonth;

    if (operation.Operation == "Add"){
      this.shouldShowDialog = true;
      param.stringDate = this.menuParam.selectedBlock.stringDate;
    }
    else if (operation.Operation == 'Edit'){
      this.shouldShowDialog = true;
      param.stringDate = this.menuParam.selectedEvent.Date;
    }
    else if (operation.Operation == 'Delete'){
      this.appointmentToDelete = param.appointment;
      this.OnDelete();
    }
    else if (operation.Operation == "View"){
      this.showViewDialog(param);
    }

    if (operation.Operation == "Add" || operation.Operation == "Edit"){
      this.showAddEditMatDialog(param);
    }
  }

  showAddEditMatDialog(dialogParam: DialogOperation){
    this.dialogResult = this.dialog.open(CalendarDialogComponent, {
      width: '500px',
      disableClose: true,
      data: { param : dialogParam}
    });

    this.dialogResult.afterClosed().subscribe(result => {
      if (result == 'add' || result == 'edit'){
        this.shouldShowDialog = false;
        this.refreshEvents();
      }
    })
  }

  showViewDialog(dialogParam: DialogOperation){
    this.dialogResult = this.dialog.open(CalendarViewDialogComponent, {
      width: '500px',
      disableClose: true,
      data: { param : dialogParam}
    });
  }

  OnDelete(){
    this.dialogResult = this.dialog.open(MessageboxComponent, {
      width:'500px',
      disableClose: true,
      data: { title: 'Delete', message: 'Are you sure you want to delete "' + this.appointmentToDelete.Title + '" appointment?'}
    })

    this.dialogResult.afterClosed().subscribe(result => {
      if (result == "ok"){
        if (this.appointmentToDelete.IsClone){
          this.eventService.deleteAppointmentRepeat(this.appointmentToDelete.Id, this.appointmentToDelete.OriginalDate).subscribe(returnData => {
            this.refreshEvents();
          });
        }
        else {
          this.eventService.deleteSchedule(this.appointmentToDelete.Id).subscribe(returnData => {
            this.refreshEvents();
          });
        }
      }
    });
  }

  refreshEvents(){
    this.generateMonth();
  }
}