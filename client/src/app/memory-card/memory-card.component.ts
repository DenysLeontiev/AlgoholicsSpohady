import { Component, Input, OnInit } from '@angular/core';
import { Memory } from '../_models/memory';
import { LikeService } from '../_services/like.service';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Component({
  selector: 'app-memory-card',
  templateUrl: './memory-card.component.html',
  styleUrls: ['./memory-card.component.css']
})
export class MemoryCardComponent implements OnInit {
  @Input() memory: Memory | undefined;

  constructor(private likeService: LikeService, private toastr: ToastrService) { }

  ngOnInit(): void {

  }

  likeMemory() {
    if (this.memory) {
      this.likeService.likeMemory(this.memory.id).subscribe((response) => {
        console.log(response);
        this.toastr.success("Ви вподобали спогад :)");
      }, (error) => {
        console.log(error);
      });
    }
  }
}
