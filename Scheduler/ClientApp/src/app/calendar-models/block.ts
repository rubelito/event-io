import { EventModel } from "./event-model";

export class Block {
    constructor(){
        this.isEmptyBlock = true;
    }

    isEmptyBlock: boolean;
    isToday: boolean = false;
    day: number = 0;
    stringDate: string = "";
    blockDate: any;
    events: EventModel[] = [];

    previousBlock: Block;
    nextBlock: Block;
}