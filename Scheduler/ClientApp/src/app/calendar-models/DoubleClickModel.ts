import { Block } from "./block";
import { EventModel } from "./event-model";

export class DoubleClickModel {
    isBlock: boolean;
    selectedBlock: Block;
    selectedAppointment: EventModel;
}