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
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize, userParams.searchTerm, userParams.orderByType);

    return this.getPaginatedResult<Memory[]>(this.baseUrl + 'memories', params);
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;

    return this.httpClient.get<T>(url, { observe: 'response', params }).pipe(
      map((response) => {
        if (response.body) {
          paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }
        return paginatedResult;
      })
    );
  }

  getPaginationHeaders(pageNumber: number, pageSize: number, searchTerm: string, orderByType: string) {
    let params = new HttpParams();
    if (pageNumber && pageSize) {

      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }

    if(searchTerm) {
      params = params.append("searchTerm", searchTerm);
    }

    if(orderByType) {
      params = params.append("orderByType", orderByType);
    }

    return params;
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
