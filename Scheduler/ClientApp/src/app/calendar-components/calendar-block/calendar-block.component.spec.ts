import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarBlockComponent } from './calendar-block.component';

describe('CalendarBlockComponent', () => {
  let component: CalendarBlockComponent;
  let fixture: ComponentFixture<CalendarBlockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarBlockComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalendarBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
