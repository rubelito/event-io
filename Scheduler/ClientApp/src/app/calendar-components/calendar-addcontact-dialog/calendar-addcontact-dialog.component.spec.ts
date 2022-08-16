import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarAddContactDialogComponent } from './calendar-addcontact-dialog.component';

describe('CalendarAdduserDialogComponent', () => {
  let component: CalendarAddContactDialogComponent;
  let fixture: ComponentFixture<CalendarAddContactDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarAddContactDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalendarAddContactDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
