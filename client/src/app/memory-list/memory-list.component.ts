import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { MemoryService } from '../_services/memory.service';
import { Memory } from '../_models/memory';
import { Pagination } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Component({
  selector: 'app-memory-list',
  templateUrl: './memory-list.component.html',
  styleUrls: ['./memory-list.component.css']
})
export class MemoryListComponent implements OnInit {

  memories: Memory[] = [];

  pagination: Pagination | undefined;
  userParams: UserParams = new UserParams();

  constructor(private memoryService: MemoryService) {
  }

  ngOnInit(): void {
    this.loadMemories();
  }

  loadMemories() {
    this.memoryService.getAllMemories(this.userParams).subscribe((response) => {
      if (response.result && response.pagination) {
        this.memories = response.result
        this.pagination = response.pagination;
      }
    }, error => {
      console.log(error);
    });
  }

  // this event is called every time we click on buttons which are responsible for pagination(we got them from bootstrap)
  pageChanged(event: any) {
    if (this.userParams && this.userParams.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMemories();
    }
  }
} 
