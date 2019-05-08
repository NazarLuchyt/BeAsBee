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
import { ChatMembersModalComponent } from './components/content/chat-members-modal/chat-members-modal.component';

import { AuthGuard } from './_guard/auth.guard';
import { routing } from './app.routing';

// Services
import { ApiService } from './_services/api.services';
import { AuthenticationService } from './_services/authentication.service';
import { JwtInterceptor } from './_services/jwt.interceptor';
import { ChatService } from './_services/chat.service';
import { MessageComponent } from './components/content/message/message.component';
import { ChatSettingComponent } from './components/content/chat-setting/chat-setting.component';
import { ModalModule } from 'ngx-bootstrap';
import { SearchComponent } from './components/common/search/search.component';
import { SelectComponent } from './components/common/select/select.component';
import { ChatConfigService } from './_services/chat-config.service';

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
    UserPreviewComponent,
    MessageComponent,
    ChatSettingComponent,
    ChatMembersModalComponent,
    SearchComponent,
    SelectComponent
  ],
  entryComponents: [
    ChatMembersModalComponent
  ],
  imports: [
    BrowserModule,
    routing,
    TabsModule.forRoot(),
    ModalModule.forRoot(),
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
    // services
    ApiService,
    AuthenticationService,
    ChatConfigService,
    ChatService,

    // Guards
    AuthGuard,

    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

// required for AOT compilation
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
