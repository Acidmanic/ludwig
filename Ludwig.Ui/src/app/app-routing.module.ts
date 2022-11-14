import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ShoeBoxComponent} from "./pages/shoe-box/shoe-box.component";
import {NotFoundComponent} from "./pages/not-found/not-found.component";

const routes: Routes = [
  {
    path:"shoe-box",
    component:ShoeBoxComponent
  },
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
