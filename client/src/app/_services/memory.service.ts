import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MemoryService {

  baseUrl: string = environment.baseUrl;
  constructor(private httpClient: HttpClient) { }

  createMemory(model: any) {
    return this.httpClient.post(this.baseUrl + "memories/add-memory", model);
  }
}
