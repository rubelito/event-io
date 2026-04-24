import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth-service/AuthService';
import { UserCredential } from 'src/app/calendar-models/user-credential';
import { DataSharingService } from 'src/app/calendar-service/DataSharingService';
import { GlobalConstants } from 'src/app/common/global-constant';

@Component({
  selector: 'app-page-home',
  templateUrl: './page-home.component.html',
  styleUrls: ['./page-home.component.css']
})
export class PageHomeComponent implements OnInit, AfterViewInit {

  firstSectionLayout: string;
  secondSectionLayout: string;
  big: boolean;

  screenshotImage = GlobalConstants.screenshotImage;
  addeditImage = GlobalConstants.addeditImage;
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

  isAutheticated: boolean = false;

  constructor(private responsive: BreakpointObserver,
    private dataSharingService: DataSharingService,
    public authService: AuthService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.initiateLayoutResponsiveness();
  }

  ngAfterViewInit(): void {
    if (!this.authService.isLoggedIn()){
      this.onAuth();
    }
    else {
      this.isAutheticated = true;
    }


    this.dataSharingService.toggleMenu.next();
  }

  onAuth(){
      let userCredential = new UserCredential();
      userCredential.Username = "john";
      userCredential.Password = "john1030";
      this.authService.logIn(userCredential).subscribe(data => {
        if (data.IsAuthenticated){
          console.log(data);
          localStorage.setItem("id", data.Id.toString());
          localStorage.setItem("username", data.Username);
          localStorage.setItem("isLogin", data.IsAuthenticated.toString());
          localStorage.setItem("loginStatus", data.LoginStatus);
          localStorage.setItem('accessToken', "basic " + data.Credential);
          localStorage.setItem('role', data.Role);
  
          this.dataSharingService.isProfilePictureChange.next(true);
          
         // setTimeout(() => {
            this.router.navigate(["/"]);
            this.isAutheticated = true;
         // }, 2000);
        }
      });
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
    });
  }
}
