import { TestBed } from '@angular/core/testing';

import { LinkguardGuard } from './linkguard.guard';

describe('LinkguardGuard', () => {
  let guard: LinkguardGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(LinkguardGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
