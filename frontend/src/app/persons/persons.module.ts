import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";

import { PersonsRoutingModule } from "./persons-routing.module";

import { ListComponent, PersonFormComponent } from "@persons";

import { ConfirmationModalComponent } from "@components";

@NgModule({
	declarations: [ListComponent, PersonFormComponent],
	imports: [CommonModule, ReactiveFormsModule, ConfirmationModalComponent, PersonsRoutingModule]
})
export class PersonsModule {}
