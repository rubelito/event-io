import { HttpClient } from "@angular/common/http";
import { createApiParams } from "../http/api-params";
import { Inject, Injectable } from "@angular/core";
import { Observable } from 'rxjs';
import { AddEditModel } from "../calendar-models/addEditModel";
import { DateModel } from "../calendar-models/dateModel";
import { EventModel } from "../calendar-models/event-model";
import { UserBasic } from "../calendar-models/user-basic";

@Injectable()
export class AppointmentService {

  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string){
    this.baseUrl = baseUrl;
    }

    getAppointments(yearMonth: string): Observable<EventModel[]> {
        return this.http.get<EventModel[]>(this.baseUrl + "schedule/GetMeetings", { params: createApiParams().set("yearMonth", yearMonth) });
    }

    getMeetings(yearMonth: string): Observable<EventModel[]> {
        return this.getAppointments(yearMonth);
    }

    getNumberOfRepeats(model: EventModel): Observable<number> {
        return this.http.post<number>(this.baseUrl + "schedule/GetNumberOfRepeats", model);
    }

    addSchedule(model: AddEditModel): Observable<string>{
        model.Appointment.After = model.Appointment.RepeatEnd != 1  ? 1 : model.Appointment.After;
        return this.http.post<string>(this.baseUrl + "schedule/CreateEvent", model);
    }

    editSchedule(model: AddEditModel): Observable<string> {
        return this.http.post<string>(this.baseUrl + "schedule/EditEvent", model);
    }

    editRepeat(model: AddEditModel): Observable<string> {
        return this.http.post<string>(this.baseUrl + "schedule/EditRepeat", model);
    }

    changeScheduleDate(model: DateModel): Observable<string> {
        return this.http.post<string>(this.baseUrl + "schedule/ChangeScheduleDate", model);
    }

    deleteSchedule(id: number): Observable<string> {
        return this.http.post<string>(this.baseUrl + "schedule/DeleteAppointment", id);
    }

    deleteAppointmentRepeat(id: number, originalDateStr: string): Observable<string> {
        return this.http.get<string>(this.baseUrl + "schedule/DeleteAppointmentRepeat", { params: createApiParams().set("appointmentId", id).set("originalDateStr", originalDateStr) });
    }
    
   getAllAttendees(appointmentId: number): Observable<UserBasic[]> {
    return this.http.get<UserBasic[]>(this.baseUrl + "schedule/GetAllAttendees", { params: createApiParams().set("appointmentId", appointmentId) });
   }
}
