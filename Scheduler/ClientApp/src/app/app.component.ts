import { Component, HostListener, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from './auth-service/AuthService';
import { MatDialog } from '@angular/material/dialog';
import { CalendarLogoutDialogComponent } from './calendar-components/calendar-logout-dialog/calendar-logout-dialog.component';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { SlideInOutAnimation } from './animations/toolbar';
import { SlideSideInOutAnimation } from './animations/sidetoolbar';
import { GlobalConstants } from './common/global-constant';
import { CalendarLoginDialogComponent } from './calendar-components/calendar-login-dialog/calendar-login-dialog.component';
import { CalendarRegisterDialogComponent } from './calendar-components/calendar-register-dialog/calendar-register-dialog.component';
import { CalendarMyprofileDialogComponent } from './calendar-components/calendar-myprofile-dialog/calendar-myprofile-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [ SlideInOutAnimation, SlideSideInOutAnimation ],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
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

  socialEmailImage = GlobalConstants.socialEmailImage;
  facebookImage = GlobalConstants.facebookImage;
  twitterImage = GlobalConstants.twitterImage;

  constructor(public authService: AuthService, private dialog: MatDialog,
     private responsive: BreakpointObserver, private router: Router) { }

  ngOnInit(): void {
    this.responsive.observe([Breakpoints.XSmall, Breakpoints.Small]).subscribe(result => {
        this.showLargeMenu = !result.matches;
    });
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
      width: '800px',
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
      width: '800px',
      disableClose: true
    });
  }

  onRegisterClick() {
    let registerDialog = this.dialog.open(CalendarRegisterDialogComponent, {
      width: '800px',
      disableClose: true
    });
  }

  onLogoutClick() {
    this.dialog.open(CalendarLogoutDialogComponent, {
      width: '200px',
      disableClose: true
    })
  }

  toggleSideMenu(){
    this.toggleMenu = !this.toggleMenu;
  }
}