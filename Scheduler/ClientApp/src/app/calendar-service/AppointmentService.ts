import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { AddEditModel } from "../calendar-models/addEditModel";
import { Appointment } from "../calendar-models/Appointment";
import { DateModel } from "../calendar-models/dateModel";
import { EventModel } from "../calendar-models/event-model";
import { UserBasic } from "../calendar-models/user-basic";

@Injectable()
export class AppointmentService {
  private headers: HttpHeaders = new HttpHeaders();
  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string){
    this.headers = this.headers.set('Content-type', 'application/json');
    this.baseUrl = baseUrl;
    }

    getAppointments(yearMonth: string) {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.get<EventModel[]>(this.baseUrl + "schedule/GetMeetings?yearMonth=" + yearMonth, { headers: this.headers});
    }

    getMeetings(yearMonth: string){
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.get<EventModel[]>(this.baseUrl + "schedule/GetMeetings?yearMonth=" + yearMonth, { headers: this.headers});
    }

    getNumberOfRepeats(model: EventModel): Observable<number> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<number>(this.baseUrl + "schedule/GetNumberOfRepeats", model, { headers: this.headers});
    }

    addSchedule(model: AddEditModel): Observable<Appointment>{
        model.Appointment.After = model.Appointment.RepeatEnd != 1  ? 1 : model.Appointment.After;
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<Appointment>(this.baseUrl + "schedule/CreateEvent", model, { headers: this.headers});
    }

    editSchedule(model: AddEditModel): Observable<Appointment> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<Appointment>(this.baseUrl + "schedule/EditEvent", model, { headers: this.headers});
    }

    editRepeat(model: AddEditModel): Observable<string> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<string>(this.baseUrl + "schedule/EditRepeat", model, { headers: this.headers});
    }

    changeScheduleDate(model: DateModel): Observable<string> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<string>(this.baseUrl + "schedule/ChangeScheduleDate", model, { headers: this.headers});
    }

    deleteSchedule(id: number): Observable<string> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<string>(this.baseUrl + "schedule/DeleteAppointment", id,  { headers: this.headers});
    }

    deleteAppointmentRepeat(id: number, originalDateStr: string): Observable<string> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.get<string>(this.baseUrl + "schedule/DeleteAppointmentRepeat?appointmentId=" + id + "&originalDateStr=" + originalDateStr,  { headers: this.headers});
    }
    
   getAllAttendees(appointmentId: number): Observable<UserBasic[]> {
    let cre = localStorage.getItem("accessToken");
    this.headers = this.headers.set('Authorization', cre!);
    return this.http.get<UserBasic[]>(this.baseUrl + "schedule/GetAllAttendees?appointmentId=" + appointmentId, { headers: this.headers});
   }
}