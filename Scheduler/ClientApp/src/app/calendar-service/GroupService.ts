import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MembersOfGroupModel } from "../calendar-models/membersOfGroup-model";
import { CreateEditGroupModel } from "../calendar-models/createEditGroupModel";
import { GroupResultModel } from "../calendar-models/groupResult-Model";
import { ResultModel } from "../calendar-models/resultMoodel";
import { UserBasic } from "../calendar-models/user-basic";
import { GroupBasic } from "../calendar-models/group-basic";

@Injectable()
export class GroupService {
    private headers: HttpHeaders = new HttpHeaders();
    private baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string){
        this.headers = this.headers.set('Content-type', 'application/json');
        this.baseUrl = baseUrl;
    }

    getAllGroupAttendees(appointmentId: number) : Observable<GroupBasic[]> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.get<GroupBasic[]>(this.baseUrl + "schedule/GetAllGroupAttendees?appointmentId=" + appointmentId, { headers: this.headers});
    }

    addGroup(model: CreateEditGroupModel) : Observable<ResultModel> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<ResultModel>(this.baseUrl + "user/creategroup", model, { headers: this.headers});
    }

    getYourGroupListWithMembers() : Observable<GroupResultModel[]>{
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.get<GroupResultModel[]>(this.baseUrl + "user/GetYourGroupListWithMembers", { headers: this.headers});
    }

    getUsersInGroup(groupId: number) : Observable<UserBasic[]>{
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.get<UserBasic[]>(this.baseUrl + "user/getUsersInGroup?groupId=" + groupId, { headers: this.headers});
    }

    addMembersToGroup(model: MembersOfGroupModel) : Observable<ResultModel> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<ResultModel>(this.baseUrl + "user/addMembersToGroup", model, { headers: this.headers});
    }

    removeMembersToGroup(model: MembersOfGroupModel) : Observable<ResultModel> {
        let cre = localStorage.getItem("accessToken");
        this.headers = this.headers.set('Authorization', cre!);
        return this.http.post<ResultModel>(this.baseUrl + "user/removeMembersToGroup", model, { headers: this.headers});
    }
}