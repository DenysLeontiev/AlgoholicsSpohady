import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Memory } from '../_models/memory';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { MemoryForUpdate } from '../_models/memoryForUpdate';
import { MemoryService } from '../_services/memory.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-memory',
  templateUrl: './edit-memory.component.html',
  styleUrls: ['./edit-memory.component.css']
})
export class EditMemoryComponent implements OnInit {

  editForm = new FormGroup({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
  })

  @Input() memory: Memory | undefined;

  model: MemoryForUpdate = {} as MemoryForUpdate;

  constructor(private memoryService: MemoryService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  updateMemory() {
    if (this.memory) {
      this.memoryService.updateMemory(this.memory?.id, this.editForm.value).subscribe((response) => {
        this.toastr.info("Spohad змінено");
        this.reloadWindow();
      }, error => {
        console.log(error);
      })
    }
  }

  reloadWindow() {
    window.location.reload();
  }
}
