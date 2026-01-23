import { TestBed } from '@angular/core/testing';

import { Llmservice } from './llmservice';

describe('Llmservice', () => {
  let service: Llmservice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Llmservice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
