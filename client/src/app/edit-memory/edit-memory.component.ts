import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Memory } from '../_models/memory';
import { NgForm } from '@angular/forms';
import { MemoryForUpdate } from '../_models/memoryForUpdate';
import { MemoryService } from '../_services/memory.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-memory',
  templateUrl: './edit-memory.component.html',
  styleUrls: ['./edit-memory.component.css']
})
export class EditMemoryComponent implements OnInit {
  @Input() memory: Memory | undefined;

  model: MemoryForUpdate = {} as MemoryForUpdate;

  constructor(private memoryService: MemoryService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  updateMemory() {
    if (this.memory) {
      this.memoryService.updateMemory(this.memory?.id, this.model).subscribe((response) => {
        this.toastr.info("Spohad змінено");
      }, error => {
        console.log(error);
      })
    }
  }
}
