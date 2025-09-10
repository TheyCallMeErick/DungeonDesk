import { TestBed } from '@angular/core/testing';

import { LoggedInUserStore } from './logged-in-user-store';

describe('LoggedInUserStore', () => {
  let service: LoggedInUserStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LoggedInUserStore);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
