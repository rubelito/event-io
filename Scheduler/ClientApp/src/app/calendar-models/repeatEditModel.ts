export class RepeatEditModel {
    Id: number;
    AppointmentId: number;
    OriginalDate: string = '';
    EditedDate: string = '';
    Title: string = '';
    Description: string = '';
    Time: string = '';
    IsDeleted: boolean;
    IsDone: boolean;
}