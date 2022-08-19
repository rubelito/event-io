import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AuthService } from './auth-service/AuthService';
import { AppointmentService } from './calendar-service/AppointmentService';
import { GroupService } from './calendar-service/GroupService';

import { LinkguardGuard } from './calendar-guard/linkguard.guard';

import { PageCalendarComponent } from './Pages/page-calendar/page-calendar';
import { CalendarBlockComponent } from './calendar-components/calendar-block/calendar-block.component';
import { CalendarEventComponent } from './calendar-components/calendar-event/calendar-event.component';
import { CalendarContextmenuComponent } from './calendar-components/calendar-contextmenu/calendar-contextmenu.component';
import { CalendarDialogComponent } from './calendar-components/calendar-dialog/calendar-dialog.component';
import { MessageboxComponent } from './calendar-components/messagebox/messagebox.component';
import { AppComponent } from './app.component';
import { PageProfileComponent } from './Pages/page-profile/page-profile.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ReactiveFormsModule } from '@angular/forms';
import {MatFormFieldModule} from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon'
import {MatChipsModule} from '@angular/material/chips';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatRadioModule} from '@angular/material/radio';
import {MatInputModule} from '@angular/material/input';
import {MatTableModule} from '@angular/material/table';
import {MatTooltipModule} from '@angular/material/tooltip';

import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PageContactsComponent } from './Pages/page-contacts/page-contacts.component';
import { CalendarLoginDialogComponent } from './calendar-components/calendar-login-dialog/calendar-login-dialog.component';
import { CalendarLogoutDialogComponent } from './calendar-components/calendar-logout-dialog/calendar-logout-dialog.component';
import { CalendarAddContactDialogComponent } from './calendar-components/calendar-addcontact-dialog/calendar-addcontact-dialog.component';
import { CalendarChangedpasswordDialogComponent } from './calendar-components/calendar-changedpassword-dialog/calendar-changedpassword-dialog.component';
import { PageGroupComponent } from './Pages/page-group/page-group.component';
import { CalendarAddeditGroupDialogComponent } from './calendar-components/calendar-addedit-group-dialog/calendar-addedit-group-dialog.component';
import { CalendarAddremoveMemberDialogComponent } from './calendar-components/calendar-addremove-member-dialog/calendar-addremove-member-dialog.component';
import { PageLoginComponent } from './Pages/page-login/page-login.component';
import { CalendarRegisterDialogComponent } from './calendar-components/calendar-register-dialog/calendar-register-dialog.component';
import { CalendarMyprofileDialogComponent } from './calendar-components/calendar-myprofile-dialog/calendar-myprofile-dialog.component';
import { PageHomeComponent } from './Pages/page-home/page-home.component';


@NgModule({
  declarations: [
    AppComponent,
    PageCalendarComponent,
    CalendarBlockComponent,
    CalendarEventComponent,
    CalendarContextmenuComponent,
    CalendarDialogComponent,
    MessageboxComponent,
    PageProfileComponent,
    PageContactsComponent,
    CalendarLoginDialogComponent,
    CalendarLogoutDialogComponent,
    CalendarAddContactDialogComponent,
    CalendarChangedpasswordDialogComponent,
    PageGroupComponent,
    CalendarAddeditGroupDialogComponent,
    CalendarAddremoveMemberDialogComponent,
    PageLoginComponent,
    CalendarRegisterDialogComponent,
    CalendarMyprofileDialogComponent,
    PageHomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: PageHomeComponent },
      { path: 'Schedules', component: PageCalendarComponent, canActivate: [LinkguardGuard] },
      { path: 'Contacts', component: PageContactsComponent, canActivate: [LinkguardGuard] },
      { path: 'Manage-group', component: PageGroupComponent, canActivate: [LinkguardGuard] }
    ]),
    BrowserAnimationsModule,
    MatNativeDateModule,
    MatDatepickerModule,
    NgxMaterialTimepickerModule,
    DragDropModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatRadioModule,
    MatTooltipModule
  ],
  providers: [
    AppointmentService,
    AuthService,
    GroupService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

