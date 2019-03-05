import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UserCreate } from 'src/app/_models/user-create.model';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { FormGroup } from '@angular/forms';


@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  errors: any;
  model: UserCreate = new UserCreate();
  loading = false;
  returnUrl: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService) { }


  @ViewChild('registrForm') registrForm: FormGroup;

  ngOnInit() { }

  registration() {
    if (this.registrForm.valid) {
      this.authenticationService.register(this.model)
        .subscribe(
          result => {
            this.router.navigate(['/login']);
          },
          response => {
            this.errors = response.error;
          }
        );
    }
  }
}
