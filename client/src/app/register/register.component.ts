import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() registerMode = new EventEmitter<boolean>();

  registerModel: any = {}

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.registerModel).subscribe((response) => {
      console.log(response);
      this.cancelRegister();
    }, error => {
      console.log(error);
    })
  }

  cancelRegister() {
    this.registerMode.emit(false);
  }
}
