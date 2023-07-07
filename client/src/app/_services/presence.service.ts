import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { UserJwt } from '../_models/userJwt';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubUrl: string = environment.hubUrl;
  private hubConnection?: HubConnection;

  constructor(private toastrService: ToastrService) { }

  startHubConnection(userJwt: UserJwt) {
    this.hubConnection = new HubConnectionBuilder()
                              .withUrl(this.hubUrl + 'presence', {accessTokenFactory: () => userJwt.token})
                              .withAutomaticReconnect()
                              .build();
    this.hubConnection.start().catch((error) => {
      console.log(error);
    })

    this.hubConnection.on("UserIsOnline", (username) => {
      this.toastrService.info(username + " увійшов на Shopahy");
    }); 

    this.hubConnection.on("UserIsOffline", (username) => {
      this.toastrService.warning(username + " вийшов з Spohadiv");
    });
  }

  stopHubConnection() {
    this.hubConnection?.stop();
  }
}
