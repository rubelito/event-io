import { HttpClient } from "@angular/common/http";
import { createApiParams } from "../http/api-params";
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

    private baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string){
        this.baseUrl = baseUrl;
    }

    getAllGroupAttendees(appointmentId: number) : Observable<GroupBasic[]> {
        return this.http.get<GroupBasic[]>(this.baseUrl + "schedule/GetAllGroupAttendees", { params: createApiParams().set("appointmentId", appointmentId) });
    }

    addGroup(model: CreateEditGroupModel) : Observable<ResultModel> {
        return this.http.post<ResultModel>(this.baseUrl + "user/creategroup", model);
    }

    getYourGroupListWithMembers() : Observable<GroupResultModel[]>{
        return this.http.get<GroupResultModel[]>(this.baseUrl + "user/GetYourGroupListWithMembers");
    }

    getUsersInGroup(groupId: number) : Observable<UserBasic[]>{
        return this.http.get<UserBasic[]>(this.baseUrl + "user/getUsersInGroup", { params: createApiParams().set("groupId", groupId) });
    }

    addMembersToGroup(model: MembersOfGroupModel) : Observable<ResultModel> {
        return this.http.post<ResultModel>(this.baseUrl + "user/addMembersToGroup", model);
    }

    removeMembersToGroup(model: MembersOfGroupModel) : Observable<ResultModel> {
        return this.http.post<ResultModel>(this.baseUrl + "user/removeMembersToGroup", model);
    }
}
