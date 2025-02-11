import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { DashboardComponent, HomeComponent } from "@pages";

const routes: Routes = [
	{
		path: "",
		component: HomeComponent
	},
	{
		path: "dashboard",
		component: DashboardComponent,
		data: {
			title: "Dashboard"
		}
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class PagesRoutingModule {}
