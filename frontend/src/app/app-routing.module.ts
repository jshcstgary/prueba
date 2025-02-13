import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { PageLayoutComponent } from "@layouts";

const routes: Routes = [
	{
		path: "",
		component: PageLayoutComponent,
		children: [
			{
				path: "",
				loadChildren: () => import("./pages/pages.module").then((m) => m.PagesModule)
			},
			{
				path: "users",
				loadChildren: () => import("./persons/persons.module").then((m) => m.PersonsModule)
			},
			{
				path: "roles",
				loadChildren: () => import("./roles/roles.module").then((m) => m.RolesModule)
			},
			{
				path: "role-options",
				loadChildren: () => import("./role-options/role-options.module").then((m) => m.RoleOptionsModule)
			}
		]
	},
	{
		path: "auth",
		loadChildren: () => import("./auth/auth.module").then((m) => m.AuthModule)
	},
	{
		path: "**",
		redirectTo: "auth"
	}
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule {}
