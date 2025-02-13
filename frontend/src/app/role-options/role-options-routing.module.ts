import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { RoleOptionListComponent } from "@role-options";

const routes: Routes = [
	{
		path: "",
		component: RoleOptionListComponent,
		data: {
			title: "Opciones de los roles"
		}
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class RoleOptionsRoutingModule {}
