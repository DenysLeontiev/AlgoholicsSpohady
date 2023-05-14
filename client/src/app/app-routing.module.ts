import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemoryCreatorComponent } from './memory-creator/memory-creator.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path:'', component: HomeComponent},
  {path:'add-memory', component: MemoryCreatorComponent, canActivate: [AuthGuard]},
  {path:'', component: HomeComponent},
  {path:'', component: HomeComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
