import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";

import { RoleOptionFormComponent, RoleOptionListComponent, RoleOptionsRoutingModule } from "@role-options";

import { ConfirmationModalComponent } from "@components";

@NgModule({
	declarations: [RoleOptionListComponent, RoleOptionFormComponent],
	imports: [CommonModule, ReactiveFormsModule, RoleOptionsRoutingModule, ConfirmationModalComponent]
})
export class RoleOptionsModule {}
