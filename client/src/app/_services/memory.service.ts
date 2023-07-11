import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Memory } from '../_models/memory';
import { UserInMemory } from '../_models/userInMemory';
import { AddUserToMemory } from '../_models/addUserToMemory';
import { RemoveUserFromMemory } from '../_models/removeUserFromMemory';
import { MemoryForUpdate } from '../_models/memoryForUpdate';
import { PaginatedResult } from '../_models/pagination';
import { map, take } from 'rxjs/operators';
import { UserParams } from '../_models/userParams';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { UserJwt } from '../_models/userJwt';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MemoryService {

  private usersInMemorySource = new BehaviorSubject<UserInMemory[]>([]);
  usersInMemory$ = this.usersInMemorySource.asObservable();

  baseUrl: string = environment.baseUrl;
  hubUrl: string = environment.hubUrl;

  private hubConnection?: HubConnection;

  constructor(private httpClient: HttpClient) { }

  startHubConnection(userJwt: UserJwt, memoryId: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + "users-in-memory?memoryId=" + memoryId, { accessTokenFactory: () => userJwt.token })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => {
      console.log(error);
    });

    this.hubConnection.on("GetUsersInMemory", (usersInMemory) => {
      this.usersInMemorySource.next(usersInMemory);
    });

    this.hubConnection.on("AddToNewToMemory", (newUserInMemory) => {
      this.usersInMemory$.pipe(take(1)).subscribe((usersInMemory) => {
        this.usersInMemorySource.next([...usersInMemory, newUserInMemory]);
      })
    });

    this.hubConnection.on("SetNewMemoryOwner", (usersInMemory) => {
      this.usersInMemorySource.next(usersInMemory);
      console.log(usersInMemory);
    });

    // this.hubConnection.on("RemoveUserFromMemory", (removedUsersFromMemory) => {
    //   this.usersInMemory$.pipe(take(1)).subscribe((removedUser) => {

    //   })
    // });

    this.hubConnection.on("RemoveUserFromMemory", (removedUser) => {
      this.usersInMemory$.pipe(take(1)).subscribe((usersInMemory) => {
        const updatedUsers = usersInMemory.filter(u => u.userName !== removedUser.userName);
        this.usersInMemorySource.next(updatedUsers);
      })
    });

    // this.hubConnection.on("RemoveUserFromMemory", (removedUsersFromMemory) => {
    //   this.usersInMemory$.pipe(take(1), map((users => {
    //     users = users.filter(u => u.userName !== removedUsersFromMemory.userName);
    //     this.usersInMemorySource.next(users);
    //     console.log(this.usersInMemorySource);
    //   })))
    // });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

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

  getUsersInMemory(memoryId: string) { // now, we are using SignalR to achieve this
    return this.httpClient.get<UserInMemory[]>(this.baseUrl + 'memories/users-in-memory/' + memoryId);
  }

  // addUserToMemory(addUserToMemory: AddUserToMemory) {
  //   return this.httpClient.post(this.baseUrl + 'memories/add-user-to-memory', addUserToMemory);
  // }

  async addUserToMemory(addUserToMemory: AddUserToMemory) {
    return this.hubConnection?.invoke("AddNewUserToMemory", addUserToMemory).catch((error) => {
      console.log(error);
    })
  }

  removeUserFromMemory(removeUserFromMemory: RemoveUserFromMemory) {
    return this.hubConnection?.invoke("RemoveUserFromMemory", removeUserFromMemory);
  }

  // removeUserFromMemory(removeUserFromMemory: RemoveUserFromMemory) {
  //   return this.httpClient.post(this.baseUrl + "memories/remove-user-from-memory", removeUserFromMemory);
  // }

  updateMemory(memoryId: string, model: any) {
    return this.httpClient.post(this.baseUrl + "memories/update-memory/" + memoryId, model);
  }

  likeMemory(memoryId: string) {
    return this.httpClient.post(this.baseUrl + 'likes/'+ memoryId, {});
  }

  // setNewOwner(memoryId: string, newOwnerId: string) {
  //   return this.httpClient.post(this.baseUrl + "memories/set-new-owner/" + memoryId + "/" + newOwnerId, {});
  // }

  async setNewOwner(memoryId: string, newOwnerId: string) {
    return this.hubConnection?.invoke("SetNewMemoryOwner", {newOwnerId: newOwnerId, memoryId: memoryId}).catch((error) => {
      console.log(error);
    });
  }
}
