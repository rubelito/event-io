import { HTTP_INTERCEPTORS, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { of, Subject } from 'rxjs';
import { AuthService } from '../auth-service/AuthService';
import { AddEditModel } from '../calendar-models/addEditModel';
import { AppointmentService } from '../calendar-service/AppointmentService';
import { ApiErrorService } from './api-error.service';
import { ApiInterceptor } from './api.interceptor';

describe('API HTTP handling', () => {
  const base = 'http://localhost/';
  let http: HttpClient;
  let requests: HttpTestingController;
  let notifications: jasmine.SpyObj<ApiErrorService>;
  let router: jasmine.SpyObj<Router>;
  let dialog: jasmine.SpyObj<MatDialog>;
  let previousStorage: { [key: string]: string };

  beforeEach(() => {
    previousStorage = { ...localStorage };
    localStorage.clear();
    notifications = jasmine.createSpyObj('ApiErrorService', ['notify']);
    router = jasmine.createSpyObj('Router', ['navigateByUrl']);
    dialog = jasmine.createSpyObj('MatDialog', ['closeAll']);
    router.navigateByUrl.and.returnValue(Promise.resolve(true));
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService, AppointmentService,
        { provide: 'BASE_URL', useValue: base },
        { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
        { provide: ApiErrorService, useValue: notifications },
        { provide: Router, useValue: router },
        { provide: MatDialog, useValue: dialog }]
    });
    http = TestBed.inject(HttpClient);
    requests = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    requests.verify();
    localStorage.clear();
    Object.keys(previousStorage).forEach(key => localStorage.setItem(key, previousStorage[key]));
  });

  it('reads the current credential for each API request', () => {
    for (const token of ['basic first', 'basic second']) {
      localStorage.setItem('accessToken', token);
      http.get(base + 'schedule/GetMeetings').subscribe();
      const req = requests.expectOne(base + 'schedule/GetMeetings');
      expect(req.request.headers.get('Authorization')).toBe(token);
      req.flush([]);
    }
  });

  it('does not send stored credentials to public endpoints, assets or other origins', () => {
    localStorage.setItem('accessToken', 'basic secret');
    for (const url of [base + 'user/Login', base + 'user/Register', base + 'user/IsEmailExist',
      base + 'assets/image.png', 'https://example.com/schedule/GetMeetings']) {
      http.get(url).subscribe();
      const req = requests.expectOne(url);
      expect(req.request.headers.has('Authorization')).toBeFalse();
      req.flush({});
    }
  });

  it('encodes query values containing plus, ampersand and hash characters', () => {
    const email = 'name+tag&other@example.com#fragment';
    TestBed.inject(AuthService).addContact(email).subscribe();
    const req = requests.expectOne(r => r.url === base + 'user/AddContact');
    expect(new URL(req.request.urlWithParams).searchParams.get('email')).toBe(email);
    req.flush({ Success: true });
  });

  it('accepts the existing JSON success string for event edits', () => {
    let result: string | undefined;
    TestBed.inject(AppointmentService).editSchedule(new AddEditModel()).subscribe(value => result = value);
    const req = requests.expectOne(base + 'schedule/EditEvent');
    expect(req.request.responseType).toBe('json');
    req.flush('Success');
    expect(result).toBe('Success');
  });

  it('preserves multipart uploads and binary avatar responses', () => {
    localStorage.setItem('accessToken', 'basic secret');
    const auth = TestBed.inject(AuthService);
    const form = new FormData();
    form.append('file', new Blob(['image']), 'avatar.png');
    auth.uploadProfileImage(form).subscribe();
    const upload = requests.expectOne(base + 'user/UploadProfilePicture');
    expect(upload.request.body).toBe(form);
    expect(upload.request.headers.has('Content-Type')).toBeFalse();
    expect(upload.request.headers.get('Authorization')).toBe('basic secret');
    upload.flush({ Success: true });
    auth.getAvatar(1).subscribe();
    const avatar = requests.expectOne(base + 'user/GetAvatar');
    expect(avatar.request.responseType).toBe('blob');
    avatar.flush(new Blob(['image']));
  });

  it('clears invalid sessions, returns home and propagates the original 401', () => {
    localStorage.setItem('accessToken', 'basic secret');
    localStorage.setItem('id', '1');
    localStorage.setItem('isLogin', 'true');
    let failure: HttpErrorResponse | undefined;
    http.get(base + 'schedule/GetMeetings').subscribe({ error: error => failure = error });
    requests.expectOne(base + 'schedule/GetMeetings').flush({}, { status: 401, statusText: 'Unauthorized' });
    expect(localStorage.getItem('accessToken')).toBeNull();
    expect(localStorage.getItem('id')).toBeNull();
    expect(localStorage.getItem('isLogin')).toBeNull();
    expect(router.navigateByUrl).toHaveBeenCalledWith('/');
    expect(dialog.closeAll).toHaveBeenCalled();
    expect(failure?.status).toBe(401);
    expect(notifications.notify).toHaveBeenCalledWith(failure!);
  });

  it('preserves a newer login when an earlier request returns 401', () => {
    localStorage.setItem('accessToken', 'basic old');
    http.get(base + 'schedule/GetMeetings').subscribe({ error: () => {} });
    localStorage.setItem('accessToken', 'basic new');
    requests.expectOne(base + 'schedule/GetMeetings').flush({}, { status: 401, statusText: 'Unauthorized' });
    expect(localStorage.getItem('accessToken')).toBe('basic new');
    expect(router.navigateByUrl).not.toHaveBeenCalled();
  });

  it('notifies and propagates server failures without retrying mutations', () => {
    let failure: HttpErrorResponse | undefined;
    http.post(base + 'schedule/EditEvent', {}).subscribe({ error: error => failure = error });
    requests.expectOne(base + 'schedule/EditEvent').flush({}, { status: 500, statusText: 'Server error' });
    expect(failure?.status).toBe(500);
    expect(notifications.notify).toHaveBeenCalledWith(failure!);
  });
});

describe('API error dialogs', () => {
  it('deduplicates concurrent errors and allows another message after closing', () => {
    const closed = new Subject<void>();
    const dialog = jasmine.createSpyObj('MatDialog', ['open']);
    dialog.open.and.returnValue({ afterClosed: () => closed });
    const service = new ApiErrorService(dialog);
    const failure = new HttpErrorResponse({ status: 500 });
    service.notify(failure);
    service.notify(failure);
    expect(dialog.open).toHaveBeenCalledTimes(1);
    closed.next();
    service.notify(failure);
    expect(dialog.open).toHaveBeenCalledTimes(2);
  });

  it('displays ASP.NET validation details', () => {
    const dialog = jasmine.createSpyObj('MatDialog', ['open']);
    dialog.open.and.returnValue({ afterClosed: () => of(undefined) });
    new ApiErrorService(dialog).notify(new HttpErrorResponse({
      status: 400, error: { errors: { Email: ['Enter a valid email address.'] } }
    }));
    expect(dialog.open.calls.mostRecent().args[1].data.message).toBe('Enter a valid email address.');
  });
});
