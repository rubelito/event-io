import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarLoginDialogComponent } from './calendar-login-dialog.component';

describe('CalendarLoginDialogComponent', () => {
  let component: CalendarLoginDialogComponent;
  let fixture: ComponentFixture<CalendarLoginDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarLoginDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalendarLoginDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
