import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() registerMode = new EventEmitter<boolean>();

  registerForm: FormGroup = new FormGroup({});

  registerModel: any = {}

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      userName:new FormControl('',Validators.required),
      email:new FormControl('', [Validators.required, Validators.email]),
      password:new FormControl('', [Validators.minLength(6), Validators.maxLength(15)]),
      confirmPassword:new FormControl('', [Validators.required, this.matchPasswordValues('password')]),
    });

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity(),
    })
  }

  matchPasswordValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null  : {notMatching: true}
    }
  }

  register() {
    console.log(this.registerForm.value);
    return;
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
