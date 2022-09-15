import { Component, OnInit, Input, Output, EventEmitter, Inject, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/auth-service/AuthService';
import { DialogOperation } from 'src/app/calendar-models/DialogOperation';
import { EventModel } from 'src/app/calendar-models/event-model';
import { AppointmentService } from 'src/app/calendar-service/AppointmentService';
import * as moment from 'moment';
import { AttendeeModel, AttendeeType } from 'src/app/calendar-models/AttendeeModel';
import { GroupService } from 'src/app/calendar-service/GroupService';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {MatAutocompleteSelectedEvent} from '@angular/material/autocomplete';
import { FormControl } from '@angular/forms';
import { map, Observable, startWith } from 'rxjs';
import { GlobalConstants } from 'src/app/common/global-constant';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { RepeatSelectionEnum } from 'src/app/calendar-models/repeatSelectionEnum';
import { RepeatEndEnum } from 'src/app/calendar-models/repeatEndEnum';
import { AddEditModel } from 'src/app/calendar-models/addEditModel';
import { MessageboxComponent } from '../messagebox/messagebox.component';

@Component({
  selector: 'app-calendar-dialog',
  templateUrl: './calendar-dialog.component.html',
  styleUrls: ['./calendar-dialog.component.css',
   '../../styles/style.css']
})
export class CalendarDialogComponent implements OnInit {
  personImageSrc = GlobalConstants.personImageSrc;
  groupImageSrc = GlobalConstants.groupImageSrc;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  
  radioGroups: string[] = ['All', 'Member', 'Group'];
  selectedRadio: string;

  repeatSelections = [RepeatSelectionEnum.None, RepeatSelectionEnum.EveryDay,
     RepeatSelectionEnum.EveryWeek, RepeatSelectionEnum.EveryMonth, RepeatSelectionEnum.EveryYear];
  selectedRepeatSelection = RepeatSelectionEnum.None;

  repeatEndSelection = [RepeatEndEnum.Never, RepeatEndEnum.After, RepeatEndEnum.OnDate];
  selectedRepeatEnd = RepeatEndEnum.Never;

  @ViewChild('attendeeInput') attendeeInput: ElementRef<HTMLInputElement>;
  
  param: DialogOperation;

  location: string ="";
  title: string = "";
  details: string = "";
  date: Date;
  time: string = "";
  createdBy: string = "";
  isOwner: boolean = true;
  after: number = 1;
  onDate: Date = new Date();
  
  selectedToRemove: AttendeeModel;
  attendees: Array<AttendeeModel> = []; 

  attendeesSelections: Array<AttendeeModel> = [];

  calendarForm = new FormControl('');
  filteredAttendees: Observable<AttendeeModel[]>;

  leftIcon = GlobalConstants.leftArrowIcon;

  columnSize: string;

  constructor(public dialogRef: MatDialogRef<CalendarDialogComponent>, private authService: AuthService, private groupService: GroupService,
    private appointmentService: AppointmentService, private responsive: BreakpointObserver,
    @Inject(MAT_DIALOG_DATA) public data: {param: DialogOperation}
    , private dialog:MatDialog) {
      this.selectedRadio = this.radioGroups[0];
     }

  private setFilter(radioVal: string){
    let tempSelections = this.attendeesSelections; 
    if (this.selectedRadio != "All"){
      tempSelections = this.attendeesSelections.filter(function(at){
        return at.Type == radioVal;
      })
    }

    this.filteredAttendees = this.calendarForm.valueChanges.pipe(
      startWith(null),
      map((atten: any | null) => (atten ? this._filter(atten) : tempSelections.slice()))
    );
  }

  private _filter(value: any): AttendeeModel[] {
    console.log(value);
    var newArray = this.attendeesSelections;
    if (value.Name == undefined){ // Make sure that it's not object
      newArray = this.attendeesSelections.filter(function(at) {
        return at.Name.toLowerCase().includes(value.toLowerCase());
      });
    }
    return newArray;
  }

  filterChange(){
    setTimeout(() => {this.setFilter(this.selectedRadio)}, 2);
  }

  ngOnInit(): void {
    this.responsiveness();
    this.param = this.data.param;

    let strDate = this.param?.stringDate as string;
    this.date = moment(strDate, 'MM/DD/YYYY').toDate();
    this.onDate = moment(strDate, 'MM/DD/YYYY').toDate();
    
    this.createdBy = this.param?.appointment.CreatedBy as string;

    if (this.param?.operation == 'Edit'){
      this.title = this.param.appointment.Title;
      this.location = this.param.appointment.Location;
      this.details = this.param.appointment.Details;
      this.time = this.param.appointment.Time;
      this.isOwner = this.param?.appointment.IsOwner as boolean;
      this.selectedRepeatSelection = this.param?.appointment.RepeatSelection;
      this.selectedRepeatEnd = this.param?.appointment.RepeatEnd;
      this.after = this.param?.appointment.After;
      this.onDate = moment(this.param?.appointment.OnDate, 'MM/DD/YYYY').toDate();
      this.onDate = this.onDate < moment("1975-01-01").toDate() ? this.date : this.onDate;
      
      this.appointmentService.getAllAttendees(this.param.appointment.Id).subscribe(data => {
        data.forEach(d => {
          var am = new AttendeeModel();
          am.Id = d.Id;
          am.Name = d.FirstName + " " + d.LastName;
          am.Type = AttendeeType.Member;

          this.attendees.push(am);
        });
      });

      this.groupService.getAllGroupAttendees(this.param.appointment.Id).subscribe(data => {
        data.forEach(d => {
          var am = new AttendeeModel();
          am.Id = d.Id;
          am.Name = d.Name;
          am.Type = AttendeeType.Group;

          this.attendees.push(am);
        });
      });
    }
    else if (this.param?.operation == 'Add'){
      this.createdBy = localStorage.getItem("username") as string;
    }

    this.authService.getContacts().subscribe(data => {
      data.forEach(d => {
        var am = new AttendeeModel();
        am.Id = d.Id;
        am.Name = d.FirstName + " " + d.LastName;
        am.Type = AttendeeType.Member;

        this.attendeesSelections.push(am);
      });

      this.groupService.getYourGroupListWithMembers().subscribe(data => {
        data.forEach(d => {
          var am = new AttendeeModel();
          am.Id = d.Id;
          am.Name = d.Name;
          am.Type = AttendeeType.Group;
  
          this.attendeesSelections.push(am);
        });

        this.setFilter(this.selectedRadio);
      })
    })
  }

  private responsiveness(){
    this.responsive.observe([
      Breakpoints.XSmall,
      Breakpoints.Small,
      Breakpoints.Medium,
      Breakpoints.Large,
      Breakpoints.XLarge]).subscribe(result => {
        if (result.breakpoints[Breakpoints.XLarge] || result.breakpoints[Breakpoints.Large] || result.breakpoints[Breakpoints.Medium]){
          this.columnSize = "1fr 1fr 1fr 1fr";
        }
        else if (result.breakpoints[Breakpoints.Small]){
          this.columnSize = "1fr 1fr";
        }
        else if (result.breakpoints[Breakpoints.XSmall]){
          this.columnSize = "1fr";
        }
    });
  }

  cancelAdd(){
    this.dialogRef.close("close");
  }

  removeAttendees(at: AttendeeModel){
    const index = this.attendees.indexOf(at);

    if (index >= 0) {
      this.attendees.splice(index, 1);
    }
  }

  selected(event: MatAutocompleteSelectedEvent){
    let selectedItem = event.option.value as AttendeeModel;
    if (selectedItem != undefined){
      let doesExist = this.attendees.some(u => u.Type == selectedItem.Type && u.Id == selectedItem.Id);

      if (doesExist){
         this.dialog.open(MessageboxComponent, {
          width:'400px',
          disableClose: true,
          data: { title: 'Alert', message: "User already invited!", hasNoCancel: true, icon: "warning"}
        })
        return;
      }
    }
    this.attendees.push(selectedItem);
    this.attendeeInput.nativeElement.value = '';
    this.calendarForm.setValue(null);
  }

  repeatEnumToDisplay(repeat: number){
    var returnVal = "None";

    if (repeat == RepeatSelectionEnum.EveryDay){
      returnVal =  "Every Day";
    }
    else if (repeat == RepeatSelectionEnum.EveryWeek){
      returnVal =  "Every Week";
    }
    else if (repeat == RepeatSelectionEnum.EveryMonth){
      returnVal =  "Every Month";
    }
    else if (repeat == RepeatSelectionEnum.EveryYear){
      returnVal =  "Every Year";
    }

    return returnVal;
  }

  repeatEndEnumToDisplay(repeat: number){
    var returnVal = "Never";

     if (repeat == RepeatEndEnum.After){
      returnVal =  "After";
    }
    else if (repeat == RepeatEndEnum.OnDate){
      returnVal =  "On Date";
    }

    return returnVal;
  }

  addEditEvent(){
    let model = new AddEditModel();
    let membersId = this.attendees.filter(a => a.Type == AttendeeType.Member).map(a => a.Id);
    let groupIds = this.attendees.filter(a => a.Type == AttendeeType.Group).map(a => a.Id);

    model.MemberIds = membersId;
    model.GroupIds = groupIds;

    if (this.param?.operation == 'Add'){
      let newAppointment = new EventModel();
      newAppointment.Title = this.title;
      newAppointment.Location =  this.location;
      newAppointment.Details = this.details;
      newAppointment.Date = moment(this.date).format("MM/DD/YYYY");
      newAppointment.Time = this.time;
      newAppointment.YearMonth = this.param.yearMonth;
      newAppointment.IsRepeat = this.selectedRepeatSelection != RepeatSelectionEnum.None;
      newAppointment.RepeatSelection = +this.selectedRepeatSelection;
      newAppointment.RepeatEnd = +this.selectedRepeatEnd;
      newAppointment.After = this.after;
      newAppointment.OnDate = moment(this.onDate).format("MM/DD/YYYY");

      model.Appointment = newAppointment;

      this.addEvent(model);
    }
    else if (this.param?.operation == 'Edit') {
      this.param.appointment.Title = this.title;
      this.param.appointment.Location = this.location;
      this.param.appointment.Details = this.details;
      this.param.appointment.Date = moment(this.date).format("MM/DD/YYYY");
      this.param.appointment.Time = this.time;
      this.param.appointment.IsRepeat = this.selectedRepeatSelection != RepeatSelectionEnum.None;
      this.param.appointment.RepeatSelection = +this.selectedRepeatSelection;
      this.param.appointment.RepeatEnd = +this.selectedRepeatEnd;
      this.param.appointment.After = this.after;
      this.param.appointment.OnDate = moment(this.onDate).format("MM/DD/YYYY");

      model.Appointment = this.param.appointment;

      this.editEvent(model);
    }
    //this.dialogRef.close(this.param);
  }

  addEvent(model: AddEditModel){
    this.appointmentService.addSchedule(model)
      .subscribe(returnData => {
        console.log(returnData);
        this.dialogRef.close('add');
      });
  }

  editEvent(model: AddEditModel){
    let confirmMessage = "Some events will be disappear, do you want to continue?";
    if (model.Appointment.IsClone == false) //Edit original
          {
            this.editOriginalEvent(model);
          }
          else if (model.Appointment.IsClone && model.Appointment.RepeatSelection == 0)//Edit clone to non repeat
          {
            this.appointmentService.getNumberOfRepeats(model.Appointment).subscribe(numberOfRepeats => {
              if (numberOfRepeats < model.Appointment.NumberOfRepeats){
                let dialogResult = this.dialog.open(MessageboxComponent, {
                  width:'500px',
                  disableClose: true,
                  data: { title: 'Delete', message: "Some events will be disappear, do you want to continue?", hasNoCancel: false, icon: "warning"}
                })
            
                dialogResult.afterClosed().subscribe(result => {
                  if (result == "ok"){
                    this.editOriginalEvent(model);
                  }
                });
              }
              else {
                this.editRepeat(model);
              }
            })
          }
          else if (model.Appointment.IsClone && model.Appointment.RepeatSelection != 0) //edit clone or edit repeat
          {
            this.appointmentService.getNumberOfRepeats(model.Appointment).subscribe(numberOfRepeats => {
              if (numberOfRepeats < model.Appointment.NumberOfRepeats){
                let dialogResult = this.dialog.open(MessageboxComponent, {
                  width:'500px',
                  disableClose: true,
                  data: { title: '', message: "Some events will be disappear, do you want to continue?", hasNoCancel: false, icon: "warning"}
                })
            
                dialogResult.afterClosed().subscribe(result => {
                  if (result == "ok"){
                    this.editOriginalEvent(model);
                  }
                });
              }
              else {
                this.editRepeat(model);
              }
            })
          }
  }

  editOriginalEvent(model: AddEditModel){
    this.appointmentService.editSchedule(model)
      .subscribe(returnData => {
          console.log(returnData);
          this.dialogRef.close('add');
        }
      );
  }

  editRepeat(model: AddEditModel){
    this.appointmentService.editRepeat(model)
      .subscribe(returnData => {
        console.log(returnData);
        this.dialogRef.close('add');
      })
  }

  onChangeAfter(val: number): void {  
    console.log(val);
  }
}