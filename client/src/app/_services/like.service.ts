import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserParams } from '../_models/userParams';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { LikedMemory } from '../_models/likedMemory';

@Injectable({
  providedIn: 'root'
})
export class LikeService {

  baseUrl: string = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  getLikedMemoriesForUser(userParams: UserParams) {
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize, userParams.searchTerm, userParams.orderByType);
    return getPaginatedResult<LikedMemory[]>(this.baseUrl + 'likes', params, this.httpClient);
  }

  likeMemory(memoryId: string) {
    return this.httpClient.post(this.baseUrl + 'likes/like-memory/'+ memoryId, {});
  }

  dislikeMemory(memoryId: string) {
    return this.httpClient.post(this.baseUrl + 'likes/dislike-memory/' + memoryId, {});
  }
}
