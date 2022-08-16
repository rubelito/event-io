import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarMyprofileDialogComponent } from './calendar-myprofile-dialog.component';

describe('CalendarMyprofileDialogComponent', () => {
  let component: CalendarMyprofileDialogComponent;
  let fixture: ComponentFixture<CalendarMyprofileDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarMyprofileDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarMyprofileDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
