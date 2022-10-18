import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class DataSharingService {
    public isProfilePictureChange: BehaviorSubject<boolean> =new BehaviorSubject<boolean>(false);
    public isShowTime: BehaviorSubject<boolean> =new BehaviorSubject<boolean>(false);
    public eventProcess: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
}
