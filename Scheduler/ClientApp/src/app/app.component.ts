import { Component, HostListener, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from './auth-service/AuthService';
import { MatDialog } from '@angular/material/dialog';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { SlideInOutAnimation } from './animations/toolbar';
import { SlideSideInOutAnimation } from './animations/sidetoolbar';
import { GlobalConstants } from './common/global-constant';
import { CalendarLoginDialogComponent } from './calendar-components/calendar-login-dialog/calendar-login-dialog.component';
import { CalendarRegisterDialogComponent } from './calendar-components/calendar-register-dialog/calendar-register-dialog.component';
import { CalendarMyprofileDialogComponent } from './calendar-components/calendar-myprofile-dialog/calendar-myprofile-dialog.component';
import { Router } from '@angular/router';
import { MessageboxComponent } from './calendar-components/messagebox/messagebox.component';
import { GlobalFuntions } from './common/global-functions';
import { DomSanitizer } from '@angular/platform-browser';
import { DataSharingService } from './calendar-service/DataSharingService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css',
   '/styles/background-animation.css',
    '/styles/top-gradiant-animation.css',
    '/styles/top-floating-animation.css'],
  animations: [ SlideInOutAnimation, SlideSideInOutAnimation ],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit  {
  showLargeMenu: boolean;
  toggleMenu: boolean = false;
  onTop: boolean = true;
  
  logoIcon = GlobalConstants.logoIcon;
  registerUserIcon = GlobalConstants.registerUserIcon;
  loginUserIcon = GlobalConstants.loginUserIcon;
  logoutUserIcon = GlobalConstants.logoutUserIcon;
  authUserIcon = GlobalConstants.authUserIcon;
  authCrossIcon = GlobalConstants.authCrossIcon;
  contactIcon = GlobalConstants.contactIcon;
  eventIcon = GlobalConstants.eventIcon;
  manageGroupIcon = GlobalConstants.manageGroupIcon;
  emptyProfileIcon = GlobalConstants.emptyProfileIcon;

  socialEmailImage = GlobalConstants.socialEmailImage;
  facebookImage = GlobalConstants.facebookImage;
  twitterImage = GlobalConstants.twitterImage;

  profilePic: any;

  constructor(public authService: AuthService, private dialog: MatDialog,
     private responsive: BreakpointObserver, private router: Router,
      private sanitizer: DomSanitizer, private dataSharingService: DataSharingService) { }

  ngOnInit(): void {
    this.responsive.observe([Breakpoints.XSmall, Breakpoints.Small]).subscribe(result => {
        this.showLargeMenu = !result.matches;
    });

    this.loadProfilePic();
  }

  loadProfilePic(){
    if (this.authService.isLoggedIn()){
      var userId = this.authService.getLoggedUserId();
    }

    this.dataSharingService.isProfilePictureChange.subscribe(change => {
      this.authService.getAvatar(userId).subscribe(result => {
        console.log(result);
        this.profilePic = GlobalFuntions.createImageFromBlob(result, this.sanitizer);
      })
    })
  }

  @HostListener('window:scroll', ['$event']) 
  kapagIniscrollNgUser(){
    this.onTop = window.scrollY == 0;
  }

  homeClicked(){
    this.router.navigate(["/"]);
  }

  onLoginClick(){
    let loginDialog = this.dialog.open(CalendarLoginDialogComponent, {
      width: '500px',
      disableClose: true
    });

    loginDialog.afterClosed().subscribe(result => {
      if (result == "onRegisterNow"){
        this.onRegisterClick();
      }
    });
  }

  showProfile(){
    let myprofileDialog = this.dialog.open(CalendarMyprofileDialogComponent, {
      width: '500px',
      disableClose: true
    });
  }

  onRegisterClick() {
    let registerDialog = this.dialog.open(CalendarRegisterDialogComponent, {
      width: '500px',
      disableClose: true
    });
  }

  onLogoutClick() {
    let dialogResult = this.dialog.open(MessageboxComponent, {
      width:'500px',
      disableClose: true,
      data: { title: 'Log Out', message: 'Are you sure you want to logout?', icon: 'logout'}
    })

    dialogResult.afterClosed().subscribe(result => {
      if (result == "ok"){
        this.authService.logOut().subscribe(resultL => {
          if (resultL == "logout"){
            this.router.navigate(["/"]);
          }
        })
      }
    });
  }

  toggleSideMenu(){
    this.toggleMenu = !this.toggleMenu;
  }
}