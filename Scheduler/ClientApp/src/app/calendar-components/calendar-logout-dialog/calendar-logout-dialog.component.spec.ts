import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarLogoutDialogComponent } from './calendar-logout-dialog.component';

describe('CalendarLogoutDialogComponent', () => {
  let component: CalendarLogoutDialogComponent;
  let fixture: ComponentFixture<CalendarLogoutDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarLogoutDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalendarLogoutDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
