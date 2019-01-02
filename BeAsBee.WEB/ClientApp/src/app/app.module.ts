import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { AuthoriseComponent } from './authorise/authorise.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { ChatComponent } from './chat/chat.component';

// Services
import { ApiService } from './_services/api.services';
import { AuthenticationService } from './_services/authentication.service';
import { HomeGuard } from './_guard/home.guard';
import { ChatPreviewComponent } from './chat-preview/chat-preview.component';
import { RegistrationComponent } from './registration/registration.component';
import { UserPreviewComponent } from './user-preview/user-preview.component';

const appRoutes: Routes = [
  { path: '', component: AuthoriseComponent },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [HomeGuard] },
  // { path: 'about', component: AboutComponent},
  // { path: '**', component: NotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    AuthoriseComponent,
    LoginComponent,
    HomeComponent,
    ChatComponent,
    ChatPreviewComponent,
    RegistrationComponent,
    UserPreviewComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes),
    TabsModule.forRoot(),
    FormsModule,
    HttpClientModule
  ],

  providers: [
    ApiService,
    AuthenticationService,
    HomeGuard,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
