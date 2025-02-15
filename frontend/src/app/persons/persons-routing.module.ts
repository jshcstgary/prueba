import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { PersonAuthFormComponent, PersonListComponent } from "@persons";

import { notAuthenticatedGuard } from "@guards";

const routes: Routes = [
	{
		path: "",
		component: PersonListComponent,
		data: {
			title: "Usuarios"
		}
	},
	{
		path: "user",
		component: PersonAuthFormComponent,
		canActivate: [notAuthenticatedGuard],
		data: {
			title: "Datos del perfil"
		}
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class PersonsRoutingModule {}
