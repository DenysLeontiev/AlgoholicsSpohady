import { Component, OnInit } from '@angular/core';
import { LikeService } from '../_services/like.service';
import { UserParams } from '../_models/userParams';
import { Pagination } from '../_models/pagination';
import { LikedMemory } from '../_models/likedMemory';

@Component({
  selector: 'app-liked-memories-list',
  templateUrl: './liked-memories-list.component.html',
  styleUrls: ['./liked-memories-list.component.css']
})
export class LikedMemoriesListComponent implements OnInit {

  pagination: Pagination | undefined;
  userParams: UserParams = new UserParams();
  likedMemories: LikedMemory[] = [];

  constructor(private likeService: LikeService) { }

  ngOnInit(): void {
    this.getLikedMemories();
  }

  getLikedMemories() {
    this.likeService.getLikedMemoriesForUser(this.userParams).subscribe((response) => {
      if (response.result && response.pagination) {
        this.pagination = response.pagination;
        this.likedMemories = response.result;
        console.log(response.result);
        console.log(response.pagination);
      }
    }, (error) => {
      console.log(error);
    });
  }

  // this event is called every time we click on buttons which are responsible for pagination(we got them from bootstrap)
  pageChanged(event: any) {
    if (this.userParams && this.userParams.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.getLikedMemories();
    }
  }
}
