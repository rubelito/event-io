export class GroupResultModel {
    Id: number = 0;
    Name: string = "";
    Owner: string = "";
    OwnerEmail: string = "";
    IsOwner: boolean = false;
    Active: boolean = false;
    Members: Member[] = [];
}

export class Member {
    LastName: string = "";
    FirstName: string = "";
    MiddleName: string = "";
}