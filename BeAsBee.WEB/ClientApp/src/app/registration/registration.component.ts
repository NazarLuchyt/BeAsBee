import { Component, OnInit } from '@angular/core';
import { UserCreate } from '../_models/user-create.model';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  error: any;
  model: UserCreate = new UserCreate();
  loading = false;
  returnUrl: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService) { }

  ngOnInit() {
   // this.authenticationService.logout();
  }
  registration() {
    this.authenticationService.register(this.model)
      .subscribe(
        result => {
          localStorage.setItem('currentUserId', result.id);
          this.router.navigate(['/home']);
        },
        response => {
          this.error = response.error.message;
        }

      );
  }
}
