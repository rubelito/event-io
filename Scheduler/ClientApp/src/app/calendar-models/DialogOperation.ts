import { ContextOperationModel } from "./contextOperation-Model";
import { EventModel } from "./event-model";

export class DialogOperation {
    appointment: EventModel = new EventModel();
    membersIds: Array<number> = [];
    groupIds: Array<number> = [];

    contextOperation: ContextOperationModel = new ContextOperationModel();
    stringDate: string = "";
    yearMonth: string = "";
}