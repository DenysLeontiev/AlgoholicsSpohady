import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemoryCreatorComponent } from './memory-creator/memory-creator.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemoryListComponent } from './memory-list/memory-list.component';
import { MemoryDetailComponent } from './memory-detail/memory-detail.component';
import { GoogleLoginComponent } from './google-login/google-login.component';
import { LikedMemoriesListComponent } from './liked-memories-list/liked-memories-list.component';

const routes: Routes = [
  {path:'', component: HomeComponent},
  {path:'add-memory', component: MemoryCreatorComponent, canActivate: [AuthGuard]},
  {path:'memories', component: MemoryListComponent, canActivate: [AuthGuard]},
  {path:'memories/:id', component: MemoryDetailComponent, canActivate: [AuthGuard]},
  {path: 'liked-memories', component: LikedMemoriesListComponent, canActivate: [AuthGuard]},
  {path:'google-account', component: GoogleLoginComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
