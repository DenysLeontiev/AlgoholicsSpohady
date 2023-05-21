import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { MemoryService } from '../_services/memory.service';
import { Memory } from '../_models/memory';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-memory-list',
  templateUrl: './memory-list.component.html',
  styleUrls: ['./memory-list.component.css']
})
export class MemoryListComponent implements OnInit {

  constructor(private memoryService: MemoryService) { }

  memories: Memory[] = [];

  pagination: Pagination | undefined;
  pageNumber: number = 1;
  pageSize: number = 5;

  ngOnInit(): void {
    this.loadMemories();
  }

  loadMemories() {
    this.memoryService.getAllMemories(this.pageNumber, this.pageSize).subscribe((response) => {
      if (response.result && response.pagination) {
        this.memories = response.result
        this.pagination = response.pagination;
      }
    }, error => {
      console.log(error);
    });
    // this.memoryService.getAllMemories().subscribe((response) => {
    //   this.memories = response;
    // }, error => {
    //   console.log(error);
    // })
  }

  // this event is called every time we click on buttons which are responsible for pagination(we got them from bootstrap)
  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMemories();
    }
  }
} 
