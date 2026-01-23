import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoryModal } from './history-modal';

describe('HistoryModal', () => {
  let component: HistoryModal;
  let fixture: ComponentFixture<HistoryModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HistoryModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistoryModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
