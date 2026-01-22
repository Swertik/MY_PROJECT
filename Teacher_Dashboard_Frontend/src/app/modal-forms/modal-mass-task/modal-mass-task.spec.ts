import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMassTask } from './modal-mass-task';

describe('ModalMassTask', () => {
  let component: ModalMassTask;
  let fixture: ComponentFixture<ModalMassTask>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalMassTask]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalMassTask);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
