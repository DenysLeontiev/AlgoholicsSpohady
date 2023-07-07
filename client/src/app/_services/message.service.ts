import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { CreateMessage } from '../_models/createMessage';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { UserJwt } from '../_models/userJwt';
import { BehaviorSubject, take } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl: string = environment.baseUrl;
  hubUrl: string = environment.hubUrl;

  private hubConnection?: HubConnection;
  private memoryMessagesSource = new BehaviorSubject<Message[]>([]);
  memoryMessages$ = this.memoryMessagesSource.asObservable();

  constructor(private httpClient: HttpClient) { }

  createHubConnection(userJwt: UserJwt, memoryId: string) {
    // debugger;
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + "message?memoryId=" + memoryId, {skipNegotiation: true, transport: signalR.HttpTransportType.WebSockets, accessTokenFactory: async () => userJwt.token })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => {
      console.log(error);
    })

    this.hubConnection.on("ReceiveMemoryMessages", (messages) => {;
      this.memoryMessagesSource.next(messages);
    });

    this.hubConnection.on("SendNewMessage", (message) => {
      this.memoryMessages$.pipe(take(1)).subscribe((messages) => { // take(1) gets an array form this.messageSource$
        this.memoryMessagesSource.next([...messages, message]);
      })
    });
  }

  stopHubConnection() {
    if(this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  // createMessage(createMessage: CreateMessage) { // 'async' word guarantee,thatthis method will return Promise
  //   return this.httpClient.post(this.baseUrl + "messages/add-message", createMessage);
  // }

  async createMessage(createMessage: CreateMessage) { // 'async' word guarantee,thatthis method will return Promise
    return this.hubConnection?.invoke("SendMessage", createMessage).catch((error) => {
      console.log(error);
    });
  }

  // getMessagesForMemory(pageNumber: number, pageSize: number, memoryId: string) {
  //   let params = getPaginationHeaders(pageNumber, pageSize);
  //   return getPaginatedResult<Message[]>(this.baseUrl + "messages/" + memoryId, params, this.httpClient);
  //   // return this.httpClient.get<Message[]>(this.baseUrl + "messages/" + memoryId);
  // }


  getMessagesForMemory(memoryId: string) {
    return this.httpClient.get<Message[]>(this.baseUrl + "messages/" + memoryId);
  }
}
