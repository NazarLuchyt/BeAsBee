import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
@Injectable()
export class HomeGuard implements CanActivate {

    constructor(private router: Router) { }


    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        var userId = localStorage.getItem('currentUserId');
        if (userId != null) {
            return true;
        } else {
            this.router.navigate(['']);
            return false;
        }
    }
}