export class Appointment {
    Id: number = 0;
    Title: string ='';
    Date: string = ''; //month is zero-based ex. 02/2022 is march 2022
    Time: string = '';
    YearMonth: string ='';
    IsOwner: boolean = false;
}