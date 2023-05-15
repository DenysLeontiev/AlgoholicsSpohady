import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Memory } from '../_models/memory';

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

  getAllMemories() {
    return this.httpClient.get<Memory[]>(this.baseUrl + 'memories');
  }
}
