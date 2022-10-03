import { AppointmentType } from "./appointmentType-enum";
import { Block } from "./block";
import { EventModel } from "./event-model";

export class ContextMenuValue {
    isOwner: boolean = false;
    isAdd: boolean = false;
    type: AppointmentType;
    show: boolean = false;
    locationX: number = 0;
    locationY: number = 0;
    selectedBlock: Block = new Block();
    selectedEvent: EventModel = new EventModel();
    isBlock: boolean = false;
}