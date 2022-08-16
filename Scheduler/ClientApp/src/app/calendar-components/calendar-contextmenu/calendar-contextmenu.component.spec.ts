import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarContextmenuComponent } from './calendar-contextmenu.component';

describe('CalendarContextmenuComponent', () => {
  let component: CalendarContextmenuComponent;
  let fixture: ComponentFixture<CalendarContextmenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarContextmenuComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalendarContextmenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
