import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarChangeavatarDialogComponent } from './calendar-changeavatar-dialog.component';

describe('CalendarChangeavatarDialogComponent', () => {
  let component: CalendarChangeavatarDialogComponent;
  let fixture: ComponentFixture<CalendarChangeavatarDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarChangeavatarDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarChangeavatarDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
