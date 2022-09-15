import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GlobalConstants } from 'src/app/common/global-constant';

@Component({
  selector: 'app-messagebox',
  templateUrl: './messagebox.component.html',
  styleUrls: ['./messagebox.component.css', '../../styles/style.css']
})
export class MessageboxComponent implements OnInit {
  title: string = "";
  message: string = "";
  hasNoCancel: boolean = false;
  icon: string = "";

  okIcon = GlobalConstants.okIcon;
  warningIcon = GlobalConstants.warningIcon;
  xIcon = GlobalConstants.xIcon;
  logoutIcon = GlobalConstants.logoutIcon;

  constructor(public dialogRef: MatDialogRef<MessageboxComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.title = this.data.title;
    this.message = this.data.message;
    this.hasNoCancel = this.data.hasNoCancel;
    this.icon = this.data.icon != undefined ? this.data.icon : "";
  }

  ok(){
    this.dialogRef.close("ok");
  }

  cancel(){
    this.dialogRef.close("cancel");
  }
}
