import { EventModel } from "./event-model";

export class Block {
    isEmptyBlock = false;
    isToday: boolean = false;
    day: number = 0;
    stringDate: string = "";
    blockDate: any;
    events: EventModel[] = [];
}