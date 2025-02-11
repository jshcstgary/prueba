import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { ListComponent } from "@persons";

const routes: Routes = [
	{
		path: "",
		component: ListComponent,
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
