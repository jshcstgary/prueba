import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { RoleListComponent } from "@roles";

const routes: Routes = [
	{
		path: "",
		component: RoleListComponent,
		data: {
			title: "Roles"
		}
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class RolesRoutingModule {}
