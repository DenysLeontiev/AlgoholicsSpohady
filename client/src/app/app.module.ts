import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './register/register.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { MemoryCreatorComponent } from './memory-creator/memory-creator.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { MemoryListComponent } from './memory-list/memory-list.component';
import { MemoryCardComponent } from './memory-card/memory-card.component';
import { MemoryDetailComponent } from './memory-detail/memory-detail.component';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { GoogleLoginProvider, SocialAuthService, SocialAuthServiceConfig } from 'angularx-social-login';
import { GoogleLoginComponent } from './google-login/google-login.component';
import { EditMemoryComponent } from './edit-memory/edit-memory.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { QrCodePopUpComponent } from './qr-code-pop-up/qr-code-pop-up.component';
import { MessagesComponent } from './messages/messages.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavComponent,
    RegisterComponent,
    MemoryCreatorComponent,
    MemoryListComponent,
    MemoryCardComponent,
    MemoryDetailComponent,
    GoogleLoginComponent,
    EditMemoryComponent,
    TextInputComponent,
    QrCodePopUpComponent,
    MessagesComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ToastrModule.forRoot({
      timeOut: 2500,
      positionClass: "toast-bottom-right",
      preventDuplicates: false,
      countDuplicates: true,
      progressBar: true,
    }),
    TabsModule.forRoot(),
    NgxGalleryModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    PaginationModule.forRoot(),
  ],
  providers: [
    SocialAuthService,
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider('933185681485-lups4j3a03jn67vmgmpd0i1kahmssgfv.apps.googleusercontent.com'),
          },
        ],
      } as SocialAuthServiceConfig,
    },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
