import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarAddeditGroupDialogComponent } from './calendar-addedit-group-dialog.component';

describe('CalendarAddeditGroupDialogComponent', () => {
  let component: CalendarAddeditGroupDialogComponent;
  let fixture: ComponentFixture<CalendarAddeditGroupDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarAddeditGroupDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarAddeditGroupDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
