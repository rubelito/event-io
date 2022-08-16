import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarAddremoveMemberDialogComponent } from './calendar-addremove-member-dialog.component';

describe('CalendarAddremoveMemberDialogComponent', () => {
  let component: CalendarAddremoveMemberDialogComponent;
  let fixture: ComponentFixture<CalendarAddremoveMemberDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarAddremoveMemberDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarAddremoveMemberDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
