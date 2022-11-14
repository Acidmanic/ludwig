import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ShoeBoxComponent} from "./pages/shoe-box/shoe-box.component";

const routes: Routes = [
  {
    path:"shoe-box",
    component:ShoeBoxComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
