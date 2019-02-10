import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Authentication } from 'src/app/_models/authentication';
import { AuthenticationService } from 'src/app/_services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  error: any;
  model: Authentication = new Authentication();
  loading = false;
  returnUrl: string;
  isLogInSubmitted = false;
  isValidModel = false;

  constructor(private route: ActivatedRoute, private router: Router, private authenticationService: AuthenticationService) {
    this.model.password = 'test2';
    //  this.model.userName = 'test2';
  }

  ngOnInit() {
    this.authenticationService.logout();
  }

  login() {
    this.isLogInSubmitted = true;
    if (this.isValidModel) {
      this.authenticationService.login(this.model)
        .subscribe(
          result => {
            this.router.navigate(['/home']);
          },
          error => {
            this.error = error.error.message;
          }

        );
    }
  }
}
