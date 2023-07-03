import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class DataSharingService {
    public isProfilePictureChange: BehaviorSubject<boolean> =new BehaviorSubject<boolean>(false);
    public isShowTime: BehaviorSubject<boolean> =new BehaviorSubject<boolean>(false);
    public eventProcess: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public tableSize: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public calendarDialogLayout: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    public toggleMenu: BehaviorSubject<void> = new BehaviorSubject<void>(undefined)
}