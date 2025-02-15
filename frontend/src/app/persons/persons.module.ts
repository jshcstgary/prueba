import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";

import { PersonAuthFormComponent, PersonFormComponent, PersonListComponent, PersonsRoutingModule } from "@persons";

import { ConfirmationModalComponent } from "@components";

@NgModule({
	declarations: [PersonAuthFormComponent, PersonFormComponent, PersonListComponent],
	imports: [CommonModule, ReactiveFormsModule, PersonsRoutingModule, ConfirmationModalComponent]
})
export class PersonsModule {}
