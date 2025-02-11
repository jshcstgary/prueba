import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";

import { AuthRoutingModule } from "./auth-routing.module";

import { AuthComponent } from "@auth";

import { LogoComponent } from "@components";

@NgModule({
	declarations: [AuthComponent],
	imports: [CommonModule, AuthRoutingModule, ReactiveFormsModule, LogoComponent]
})
export class AuthModule {}
