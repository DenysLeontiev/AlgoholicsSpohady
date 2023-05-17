import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GoogleLoginProvider, SocialAuthService, SocialUser } from 'angularx-social-login';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  isRegisterMode: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  registerToggle() {
    this.isRegisterMode = !this.isRegisterMode;
  }
}
