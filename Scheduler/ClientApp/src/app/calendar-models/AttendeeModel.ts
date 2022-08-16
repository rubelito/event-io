export class AttendeeModel {
    Id: number = 0;
    Name: string = "";
    Type: AttendeeType;
}

export enum AttendeeType {
    Member = "Member",
    Group = "Group"
}
