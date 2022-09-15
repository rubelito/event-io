import { RepeatEditModel } from "./repeatEditModel";

export class EventModel {
    Id: number = 0;
    Location: string = "";
    Title: string ='';
    Details: string = '';
    OriginalDate: string = '01/01/0001';
    Date: string = '';
    Time: string = '';
    YearMonth: string ='';
    IsRepeat: boolean = false;
    IsOwner: boolean = false;
    CreatedBy: string = '';
    CreatorId: number = 0;

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