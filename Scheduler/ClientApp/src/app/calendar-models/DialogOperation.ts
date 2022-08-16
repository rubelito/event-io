import { EventModel } from "./event-model";

export class DialogOperation {
    appointment: EventModel = new EventModel();
    membersIds: Array<number> = [];
    groupIds: Array<number> = [];

    operation: string = "";
    stringDate: string = "";
    yearMonth: string = "";
}