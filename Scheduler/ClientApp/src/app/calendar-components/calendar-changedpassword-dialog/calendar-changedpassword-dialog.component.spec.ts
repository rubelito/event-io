import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarChangedpasswordDialogComponent } from './calendar-changedpassword-dialog.component';

describe('CalendarChangedpasswordDialogComponent', () => {
  let component: CalendarChangedpasswordDialogComponent;
  let fixture: ComponentFixture<CalendarChangedpasswordDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarChangedpasswordDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarChangedpasswordDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
