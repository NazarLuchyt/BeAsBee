import { BrowserModule } from '@angular/platform-browser';
// import ngx-translate and the http loader
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
// Components
import { AuthoriseComponent } from './components/authorization/authorise/authorise.component';
import { LoginComponent } from './components/authorization/login/login.component';
import { HomeComponent } from './components/content/home/home.component';
import { AppComponent } from './app.component';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { ChatComponent } from './components/content/chat/chat.component';
import { ChatPreviewComponent } from './components/content/chat-preview/chat-preview.component';
import { RegistrationComponent } from './components/authorization/registration/registration.component';
import { UserPreviewComponent } from './components/content/user-preview/user-preview.component';

// Services
import { ApiService } from './_services/api.services';
import { AuthenticationService } from './_services/authentication.service';
import { HomeGuard } from './_guard/home.guard';



const appRoutes: Routes = [
  { path: '', component: AuthoriseComponent },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [HomeGuard] }
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
    TranslateModule.forRoot(
      {
        loader:
        {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
        }
      }),
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

// required for AOT compilation
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
