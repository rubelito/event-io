import { Component, OnInit, Input, Output, EventEmitter, Inject, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/auth-service/AuthService';
import { DialogOperation } from 'src/app/calendar-models/DialogOperation';
import { EventModel } from 'src/app/calendar-models/event-model';
import { AppointmentService } from 'src/app/calendar-service/AppointmentService';
import * as moment from 'moment';
import { AttendeeModel, AttendeeType } from 'src/app/calendar-models/AttendeeModel';
import { GroupService } from 'src/app/calendar-service/GroupService';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import {MatAutocompleteSelectedEvent} from '@angular/material/autocomplete';
import { FormControl } from '@angular/forms';
import { map, Observable, startWith } from 'rxjs';
import { GlobalConstants } from 'src/app/common/global-constant';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

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

  @ViewChild('attendeeInput') attendeeInput: ElementRef<HTMLInputElement>;
  
  param: DialogOperation;

  location: string ="";
  title: string = "";
  details: string = "";
  date: Date;
  time: string = "";
  createdBy: string = "";
  isOwner: boolean = true;

  selectedToRemove: AttendeeModel;
  attendees: Array<AttendeeModel> = []; 

  attendeesSelections: Array<AttendeeModel> = [];

  calendarForm = new FormControl('');
  filteredAttendees: Observable<AttendeeModel[]>;

  leftIcon = GlobalConstants.leftArrowIcon;

  columnSize: string;

  constructor(public dialogRef: MatDialogRef<CalendarDialogComponent>, private authService: AuthService, private groupService: GroupService,
    private appointmentService: AppointmentService, private responsive: BreakpointObserver,
    @Inject(MAT_DIALOG_DATA) public data: {param: DialogOperation}) {
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
    this.createdBy = this.param?.appointment.CreatedBy as string;

    if (this.param?.operation == 'Edit'){
      this.title = this.param.appointment.Title;
      this.location = this.param.appointment.Location;
      this.details = this.param.appointment.Details;
      this.time = this.param.appointment.Time;
      this.isOwner = this.param?.appointment.IsOwner as boolean;

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

  addEditEvent(){
    if (this.param?.operation == 'Add'){
      let newAppointment = new EventModel();
      newAppointment.Title = this.title;
      newAppointment.Location =  this.location;
      newAppointment.Details = this.details;
      newAppointment.Date = moment(this.date).format("MM/DD/YYYY");
      newAppointment.Time = this.time;
      newAppointment.YearMonth = this.param.yearMonth;
      this.param.appointment = newAppointment;
    }
    else if (this.param?.operation == 'Edit') {
      this.param.appointment.Title = this.title;
      this.param.appointment.Location = this.location;
      this.param.appointment.Details = this.details;
      this.param.appointment.Date = moment(this.date).format("MM/DD/YYYY");
      this.param.appointment.Time = this.time;
    }

    this.param!.membersIds = this.attendees.filter(a => a.Type == AttendeeType.Member).map(a => a.Id);
    this.param!.groupIds = this.attendees.filter(a => a.Type == AttendeeType.Group).map(a => a.Id);

    this.dialogRef.close(this.param);
  }

  removeAttendees(at: AttendeeModel){
    const index = this.attendees.indexOf(at);

    if (index >= 0) {
      this.attendees.splice(index, 1);
    }
    console.log(this.attendees);
    /* this.attendees = this.attendees.filter(iu => {
      var tobeReturned = true;

      if (iu.Id == this.selectedToRemove.Id){
        if (iu.Type == this.selectedToRemove.Type){
          tobeReturned = false;
        }
      }

      return tobeReturned;
    }); */
  }

  selected(event: MatAutocompleteSelectedEvent){
    let selectedItem = event.option.value as AttendeeModel;
    if (selectedItem != undefined){
      let doesExist = this.attendees.some(u => u.Type == selectedItem.Type && u.Id == selectedItem.Id);

      if (doesExist){
        alert("Already invited!");
        return;
      }
    }
    this.attendees.push(selectedItem);
    this.attendeeInput.nativeElement.value = '';
    this.calendarForm.setValue(null);
  }
}