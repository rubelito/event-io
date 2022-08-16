import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarRegisterDialogComponent } from './calendar-register-dialog.component';

describe('CalendarRegisterDialogComponent', () => {
  let component: CalendarRegisterDialogComponent;
  let fixture: ComponentFixture<CalendarRegisterDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarRegisterDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
