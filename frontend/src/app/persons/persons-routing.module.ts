import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { PersonListComponent } from "@persons";

const routes: Routes = [
	{
		path: "",
		component: PersonListComponent,
		data: {
			title: "Usuarios"
		}
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class PersonsRoutingModule {}
