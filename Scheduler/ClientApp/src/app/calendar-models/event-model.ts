import { AppointmentType } from "./appointmentType-enum";

export class EventModel {
    Id: number = 0;
    Location: string = "";
    Title: string ='';
    Color: string = '';
    Details: string = '';
    OriginalDate: string = '01/01/0001';
    Date: string = '';
    Time: string = '';
    YearMonth: string ='';
    IsRepeat: boolean = false;
    IsOwner: boolean = false;
    CreatedBy: string = '';
    CreatorId: number = 0;
    Type: AppointmentType;

    IsDone: boolean = false;
    IsDeleted: boolean = false;
    HasEdit: boolean = false;

    RepeatSelection: number = 0;
    RepeatEnd: number = 0;

    After: number = 0;
    OnDate: string = '01/01/0001';
    IsClone: boolean = false;
    NumberOfRepeats: number = 0;
}