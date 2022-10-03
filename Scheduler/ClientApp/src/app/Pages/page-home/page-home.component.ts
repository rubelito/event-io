import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { GlobalConstants } from 'src/app/common/global-constant';

@Component({
  selector: 'app-page-home',
  templateUrl: './page-home.component.html',
  styleUrls: ['./page-home.component.css']
})
export class PageHomeComponent implements OnInit {

  firstSectionLayout: string;
  secondSectionLayout: string;
  big: boolean;

  screenshotImage = GlobalConstants.screenshotImage;
  chromeBrowserIcon = GlobalConstants.chromeBrowserIcon;
  firefoxIcon = GlobalConstants.firefoxIcon;
  ieIcon = GlobalConstants.ieIcon;
  safaryIcon = GlobalConstants.safaryIcon;
  appleIcon = GlobalConstants.appleIcon;
  androidIcon = GlobalConstants.androidIcon;
  windowsIcon = GlobalConstants.windowsIcon;
  tabletImage = GlobalConstants.tabletsImage;

  appointmentImage = GlobalConstants.appointmentImageSrc;
  taskImage = GlobalConstants.taskImageSrc;
  reminderImage = GlobalConstants.reminderImageSrc;

  colorImage = GlobalConstants.colorImage;
  contactImage = GlobalConstants.contactImage;
  responsiveImage = GlobalConstants.responsiveImage;
  privacyImage = GlobalConstants.privacyImage;
  groupImage = GlobalConstants.groupImage;
  reoccurImage = GlobalConstants.reoccurImage;

  constructor(private responsive: BreakpointObserver) { }

  ngOnInit(): void {
    this.initiateLayoutResponsiveness();
  }

  initiateLayoutResponsiveness(){
    this.responsive.observe([
      Breakpoints.XLarge,
      Breakpoints.Large,
      Breakpoints.Medium,
      Breakpoints.Small,
      Breakpoints.XSmall
    ]).subscribe(result => {
      if (result.breakpoints[Breakpoints.XLarge] || result.breakpoints[Breakpoints.Large]
         || result.breakpoints[Breakpoints.Medium]){
        this.firstSectionLayout = "browser-large";
        this.secondSectionLayout = "feature-top-large";
        this.big = true;
      }
      else if (result.breakpoints[Breakpoints.Small] || result.breakpoints[Breakpoints.XSmall]){
        this.firstSectionLayout = "browser-small";
        this.secondSectionLayout = "feature-top-small";
        this.big = false; 
      }

      if (result.breakpoints[Breakpoints.XLarge]){
        console.log("XLarge");
      }
      else if (result.breakpoints[Breakpoints.Large]){
        console.log("Large");
      }
      else if (result.breakpoints[Breakpoints.Medium]){
        console.log("Medium");
      }
      else if (result.breakpoints[Breakpoints.Small]){
        console.log("Small");
      }
      else if (result.breakpoints[Breakpoints.XSmall]){
        console.log("XSmall");
      }
    });
  }
}
