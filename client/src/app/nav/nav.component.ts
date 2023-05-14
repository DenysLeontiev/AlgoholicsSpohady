import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  loginModel:any = {};

  constructor(public accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.loginModel).subscribe((response) => {
      this.toastr.success("Ласкаво просимо до Спогадів")
      console.log(response);
    }, error => {
      console.log(error);
    })
  }

  logout() {
    this.accountService.logout();
  }
}
