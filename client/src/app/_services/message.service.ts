import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { CreateMessage } from '../_models/createMessage';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl: string = environment.baseUrl;

  constructor(private httpClient: HttpClient) { }

  createMessage(createMessage: CreateMessage) {
    return this.httpClient.post(this.baseUrl + "messages/add-message", createMessage);
  }

  getMessagesForMemory(pageNumber: number,pageSize: number, memoryId: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    return getPaginatedResult<Message[]>(this.baseUrl + "messages/"+memoryId, params, this.httpClient);
    // return this.httpClient.get<Message[]>(this.baseUrl + "messages/"+memoryId);
  }
}
