import { Routes, RouterModule } from '@angular/router';
import { AuthoriseComponent } from './components/authorization/authorise/authorise.component';
import { LoginComponent } from './components/authorization/login/login.component';
import { HomeComponent } from './components/content/home/home.component';
import { AuthGuard } from './_guard/auth.guard';

const appRoutes: Routes = [
    { path: '', component: AuthoriseComponent },
    { path: 'login', component: LoginComponent },
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard] }
];

export const routing = RouterModule.forRoot(appRoutes);
