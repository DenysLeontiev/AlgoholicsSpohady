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

@Injectable({
  providedIn: 'root'
})
export class MemoryService {

  baseUrl: string = environment.baseUrl;
  paginatedResult: PaginatedResult<Memory[]> = new PaginatedResult<Memory[]>;

  constructor(private httpClient: HttpClient) { }

  createMemory(model: any) {
    return this.httpClient.post(this.baseUrl + "memories/add-memory", model);
  }

  getMemory(id: string) {
    return this.httpClient.get<Memory>(this.baseUrl + "memories/" + id);
  }

  getAllMemories(pageNumber?: number, itemsPerPage?: number) {
    let params = new HttpParams();
    if (pageNumber && itemsPerPage) {

      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', itemsPerPage);
    }



    return this.httpClient.get<Memory[]>(this.baseUrl + 'memories', {observe: 'response', params}).pipe(
      map((response) => {
        if(response.body) {
          this.paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if(pagination) {
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult;
      })
    );
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
