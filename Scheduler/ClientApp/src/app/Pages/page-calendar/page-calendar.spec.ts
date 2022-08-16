import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { PageCalendarComponent } from './page-calendar';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule
      ],
      declarations: [
        PageCalendarComponent
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(PageCalendarComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'scheduler-app'`, () => {
    const fixture = TestBed.createComponent(PageCalendarComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('scheduler-app');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(PageCalendarComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.content span')?.textContent).toContain('scheduler-app app is running!');
  });
});
