import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";

import { RoleFormComponent, RoleListComponent, RolesRoutingModule } from "@roles";

import { ConfirmationModalComponent } from "@components";

@NgModule({
	declarations: [RoleListComponent, RoleFormComponent],
	imports: [CommonModule, ReactiveFormsModule, RolesRoutingModule, ConfirmationModalComponent]
})
export class RolesModule {}
