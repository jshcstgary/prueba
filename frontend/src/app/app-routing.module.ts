import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { PageLayoutComponent } from "@layouts";

import { NotAuthorizedComponent } from "@components";

import { authenticatedGuard, notAuthenticatedGuard } from "@guards";

const routes: Routes = [
	{
		path: "",
		component: PageLayoutComponent,
		children: [
			{
				path: "",
				loadChildren: () => import("./pages/pages.module").then((m) => m.PagesModule),
				canActivate: [notAuthenticatedGuard]
			},
			{
				path: "users",
				loadChildren: () => import("./persons/persons.module").then((m) => m.PersonsModule),
				canActivate: [notAuthenticatedGuard]
			},
			{
				path: "roles",
				loadChildren: () => import("./roles/roles.module").then((m) => m.RolesModule),
				canActivate: [notAuthenticatedGuard]
			},
			{
				path: "role-options",
				loadChildren: () => import("./role-options/role-options.module").then((m) => m.RoleOptionsModule),
				canActivate: [notAuthenticatedGuard]
			}
		]
	},
	{
		path: "auth",
		loadChildren: () => import("./auth/auth.module").then((m) => m.AuthModule),
		canActivate: [authenticatedGuard]
	},
	{
		path: "not-authorized",
		component: NotAuthorizedComponent
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
