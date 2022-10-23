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
import { RangeType } from 'src/app/calendar-models/rangeType-enum';
import { CalendarDeleteDialogComponent } from 'src/app/calendar-components/calendar-delete-dialog/calendar-delete-dialog.component';
import { DataSharingService } from 'src/app/calendar-service/DataSharingService';

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

  eventRanges: EventModel[] = [];

  constructor(private eventService: AppointmentService,
     private dialog:MatDialog, private dataSharingService: DataSharingService){
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
      block.isEmptyBlock = false;

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

    //Make reference to previous and next Block

    for (let i = 0; i <= this.daysBlock.length - 1; i++){
      if (i != 0 && i != this.daysBlock.length - 1){
        this.daysBlock[i].previousBlock = this.daysBlock[i - 1];
        this.daysBlock[i].nextBlock = this.daysBlock[i + 1];
      }
    }

    this.getEvents();
  }

  getEvents(){
    this.eventService.getAppointments(this.yearMonth)
      .subscribe((data: EventModel[]) =>
        {
          this.determineRange(data);
          data = data.concat(this.eventRanges);
          this.putToBlocks(data);
        } 
      );
  }

  determineRange(event:EventModel[]){
    event.forEach(d => {
      d.IsEmptyEvent = false;
      if (d.EndDateSpan == 0){
        d.IsRange = false;
        d.RangeType = RangeType.NoRange;
      }
      else {
        var tempRangeList: EventModel[] = [];
        d.IsRange = true;
        d.RangeType = RangeType.Start;
        d.MainEventReference = d;
        
        for (let i = 1; i <= d.EndDateSpan; i++) {
          var tempRange = new EventModel();
          tempRange.IsRange = true;
          tempRange.Id = d.Id;
          tempRange.Date = moment(d.Date).add(i, 'days').format("MM/DD/YYYY");
          tempRange.Time = d.Time;
          tempRange.YearMonth = d.YearMonth;
          tempRange.Color = d.Color;
          tempRange.IsOwner = d.IsOwner;
          tempRange.MainEventReference = d;
          tempRange.RangeType = RangeType.Middle;
          tempRange.IsClone = d.IsClone;
          tempRange.IsDone = d.IsDone;
          
          tempRangeList.push(tempRange);
        }
        tempRangeList[tempRangeList.length - 1].RangeType = RangeType.End
        this.eventRanges = this.eventRanges.concat(tempRangeList);

      }
    });
  }

  putToBlocks(event: EventModel[]){
    this.daysBlock.forEach(b => {
      let tempEvent = event.filter((ev) => {
        return ev.Date == b.stringDate;
      })
      
      let eventWithRange = tempEvent.filter((ev) => {
        return ev.IsRange;
      });
      eventWithRange.sort(function(a, b) {
        return Date.parse('1970/01/01 ' + a.Time.slice(0, -2) + ' ' + a.Time.slice(-2)) - Date.parse('1970/01/01 ' + b.Time.slice(0, -2) + ' ' + b.Time.slice(-2))
      });

      //Sort events by time
      let eventWithoutRange = tempEvent.filter((ev) => {
        return ev.IsRange == false;
      });
      eventWithoutRange.sort(function(a, b) {
        return Date.parse('1970/01/01 ' + a.Time.slice(0, -2) + ' ' + a.Time.slice(-2)) - Date.parse('1970/01/01 ' + b.Time.slice(0, -2) + ' ' + b.Time.slice(-2))
      });

      b.events = eventWithRange.concat(eventWithoutRange);
    })
    this.dataSharingService.eventProcess.next(true);
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
    if (this.appointmentToDelete.IsClone){
      this.dialogResult = this.dialog.open(CalendarDeleteDialogComponent, {
        width: '400px'
      });
  
      this.dialogResult.afterClosed().subscribe(result => {
        if (result == "deleteAll"){
          this.eventService.deleteSchedule(this.appointmentToDelete.Id).subscribe(returnData => {
            this.refreshEvents();
          });
        }
        else if (result = "deleteOne"){
          this.eventService.deleteAppointmentRepeat(this.appointmentToDelete.Id, this.appointmentToDelete.OriginalDate).subscribe(returnData => {
            this.refreshEvents();
          });
        }
      });
    }
    else {
      this.dialogResult = this.dialog.open(MessageboxComponent, {
        width:'400px',
        disableClose: true,
        data: { title: 'Delete', message: 'Are you sure you want to delete "' + this.appointmentToDelete.Title + '" appointment?'}
      })

      this.dialogResult.afterClosed().subscribe(result => {
        if (result == "ok"){
          this.eventService.deleteSchedule(this.appointmentToDelete.Id).subscribe(returnData => {
            this.refreshEvents();
          });
        }
      });
    }
  }

  refreshEvents(){
    this.eventRanges = [];
    this.generateMonth();
  }
}
