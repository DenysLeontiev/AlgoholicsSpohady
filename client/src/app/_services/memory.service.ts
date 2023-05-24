import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Memory } from '../_models/memory';
import { UserInMemory } from '../_models/userInMemory';
import { AddUserToMemory } from '../_models/addUserToMemory';
import { RemoveUserFromMemory } from '../_models/removeUserFromMemory';
import { MemoryForUpdate } from '../_models/memoryForUpdate';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { UserParams } from '../_models/userParams';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MemoryService {

  baseUrl: string = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  createMemory(model: any) {
    return this.httpClient.post(this.baseUrl + "memories/add-memory", model);
  }

  getMemory(id: string) {
    return this.httpClient.get<Memory>(this.baseUrl + "memories/" + id);
  }

  getAllMemories(userParams: UserParams) {
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize, userParams.searchTerm, userParams.orderByType);

    return getPaginatedResult<Memory[]>(this.baseUrl + 'memories', params, this.httpClient);
  }

 

  getUsersInMemory(memoryId: string) {
    return this.httpClient.get<UserInMemory[]>(this.baseUrl + 'memories/users-in-memory/' + memoryId);
  }

  addUserToMemory(addUserToMemory: AddUserToMemory) {
    return this.httpClient.post(this.baseUrl + 'memories/add-user-to-memory', addUserToMemory);
  }

  removeUserFromMemory(removeUserFromMemory: RemoveUserFromMemory) {
    return this.httpClient.post(this.baseUrl + "memories/remove-user-from-memory", removeUserFromMemory);
  }

  updateMemory(memoryId: string, model: MemoryForUpdate) {
    return this.httpClient.post(this.baseUrl + "memories/update-memory/" + memoryId, model);
  }
}
