import { Component, input } from "@angular/core";
import { RouterModule } from "@angular/router";

import { LogoComponent } from "@components";

import { RoleOption } from "@types";

@Component({
	selector: "app-sidebar",
	imports: [RouterModule, LogoComponent],
	templateUrl: "./sidebar.component.html"
})
export class SidebarComponent {
	public isLoading = input.required<boolean>();

	public roleOptions = input.required<RoleOption[]>();
}
