import { Component, Input, OnInit } from '@angular/core';
import { LikedMemory } from '../_models/likedMemory';
import { LikeService } from '../_services/like.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-liked-memory',
  templateUrl: './liked-memory.component.html',
  styleUrls: ['./liked-memory.component.css']
})
export class LikedMemoryComponent implements OnInit { 
  @Input() likedMemory: LikedMemory | undefined; //  @Input() likedMemory?: LikedMemory; is there any difference???

  constructor(private likeService: LikeService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  dislikeMemory() {
    if(this.likedMemory) {
      this.likeService.dislikeMemory(this.likedMemory.id).subscribe((response) => {
        this.toastr.info("Цей Spohad вам більше не дов подоби :(");
      }, (error) => {
        console.log(error);
      })
    }
  }
}
