import { TestBed } from '@angular/core/testing';

import { LibraryApiService } from './library-api.service';

describe('LibraryApiService', () => {
  let service: LibraryApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LibraryApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
