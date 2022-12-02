import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ShoeBoxComponent} from "./pages/shoe-box/shoe-box.component";
import {NotFoundComponent} from "./pages/not-found/not-found.component";
import {LoginComponent} from "./pages/login/login.component";
import {ConfigurationsComponent} from "./pages/configurations/configurations.component";

const routes: Routes = [
  {path:"shoe-box", component:ShoeBoxComponent},
  {path:"configurations", component:ConfigurationsComponent},
  {path:"login", component:LoginComponent},
  {path:"", component:ShoeBoxComponent},
  {path:"#", component:ShoeBoxComponent},
  { path: '404', component: NotFoundComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
