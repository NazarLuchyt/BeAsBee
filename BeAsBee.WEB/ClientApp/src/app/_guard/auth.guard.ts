import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private auth: AuthenticationService) { }

    canActivate(): boolean {
        const userId = localStorage.getItem('currentUserGuid');
        if (!this.auth.isAuthenticated() && !userId) {
            this.router.navigate(['']);
            return false;
        } else {
            return true;
        }
    }
}
