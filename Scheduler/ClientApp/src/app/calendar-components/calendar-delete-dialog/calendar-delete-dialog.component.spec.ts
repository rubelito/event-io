import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarDeleteDialogComponent } from './calendar-delete-dialog.component';

describe('CalendarDeleteDialogComponent', () => {
  let component: CalendarDeleteDialogComponent;
  let fixture: ComponentFixture<CalendarDeleteDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarDeleteDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarDeleteDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
