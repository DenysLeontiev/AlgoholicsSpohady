import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { MemoryService } from '../_services/memory.service';
import { Memory } from '../_models/memory';

@Component({
  selector: 'app-memory-list',
  templateUrl: './memory-list.component.html',
  styleUrls: ['./memory-list.component.css']
})
export class MemoryListComponent implements OnInit {

  constructor(private memoryService: MemoryService) { }

  memories: Memory[] = [];

  ngOnInit(): void {
    this.loadMemories();
  }

  loadMemories() {
    this.memoryService.getAllMemories().subscribe((response) => {
      this.memories = response;
    }, error => {
      console.log(error);
    })
  }
} 
