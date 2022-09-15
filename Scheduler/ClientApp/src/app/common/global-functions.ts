import { DomSanitizer } from "@angular/platform-browser";

export class GlobalFuntions {
    public static createImageFromBlob(blob: Blob, sanitizer: DomSanitizer): any {
        const objectURL = URL.createObjectURL(blob);
        const img = sanitizer.bypassSecurityTrustUrl(objectURL);
        return img;
    }
}