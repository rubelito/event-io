import { DomSanitizer } from "@angular/platform-browser";
import { AppointmentType } from "../calendar-models/appointmentType-enum";

export class GlobalFuntions {
    public static createImageFromBlob(blob: Blob, sanitizer: DomSanitizer): any {
        const objectURL = URL.createObjectURL(blob);
        const img = sanitizer.bypassSecurityTrustUrl(objectURL);
        return img;
    }

    public static ConvertAppoinmentTypeToString(type: AppointmentType): string{
        let strType = "";

        if (type == AppointmentType.Appointment){
            strType  = "Appointment";
        }
        else if (type == AppointmentType.Task){
            strType = "Task";
        }
        else if (type == AppointmentType.Reminder){
            strType = "Reminder";
        }

        return strType;
    }

    public static ConvertStringToAppoinmentType(strType: string): AppointmentType {
        var type: AppointmentType = AppointmentType.Appointment;
        
        if (strType == "Task"){
            type = AppointmentType.Task;
        }
        else if (strType == "Reminder"){
            type = AppointmentType.Reminder;
        }

        return type;
    }
}