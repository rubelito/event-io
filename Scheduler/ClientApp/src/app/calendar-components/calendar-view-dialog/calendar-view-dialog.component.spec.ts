import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarViewDialogComponent } from './calendar-view-dialog.component';

describe('CalendarViewDialogComponent', () => {
  let component: CalendarViewDialogComponent;
  let fixture: ComponentFixture<CalendarViewDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarViewDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarViewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
