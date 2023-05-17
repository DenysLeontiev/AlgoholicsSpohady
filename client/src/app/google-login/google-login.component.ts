import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { CredentialResponse } from 'google-one-tap';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-google-login',
  templateUrl: './google-login.component.html',
  styleUrls: ['./google-login.component.css']
})
export class GoogleLoginComponent implements OnInit {

  constructor(private accountService: AccountService,
              private router: Router,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    // @ts-ignore
    window.onGoogleLibraryLoad = () => {
      // @ts-ignore
      google.accounts.id.initialize({
        client_id: environment.clientId,
        callback: this.handleCredentialResponse.bind(this),
        auto_select: false,
        cancel_on_tap_outside: true
      });
      // @ts-ignore
      google.accounts.id.renderButton(
        // @ts-ignore
        document.getElementById("buttonDiv"),
        { theme: "outline", size: "large", width: "100%" }
      );
      // @ts-ignore
      google.accounts.id.prompt((notification: PromptMomentNotification) => { });
    };
  }
  async handleCredentialResponse(response: CredentialResponse) {
    await this.accountService.LoginWithGoogle(response.credential).subscribe(
      (x: any) => {
        this.accountService.setCurrentUser(x);
          this.router.navigateByUrl("/memories");
          this.toastr.success("Ласкаво просимо");
      },
      (error: any) => {
        console.log(error);
        this.toastr.error("Потрібно створити аккаунт,щоб мати змогу зареєструватися через Google");
      }
    );

  }
}
