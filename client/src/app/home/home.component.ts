import { Component, EventEmitter, OnInit, Output } from '@angular/core';

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
