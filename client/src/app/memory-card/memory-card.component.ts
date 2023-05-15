import { Component, Input, OnInit } from '@angular/core';
import { Memory } from '../_models/memory';

@Component({
  selector: 'app-memory-card',
  templateUrl: './memory-card.component.html',
  styleUrls: ['./memory-card.component.css']
})
export class MemoryCardComponent implements OnInit {
  @Input() memory: Memory | undefined;

  constructor() { }

  ngOnInit(): void {
  }

}
