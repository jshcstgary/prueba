import { Component } from "@angular/core";
import { RouterModule } from "@angular/router";

import { LogoComponent } from "@components";

@Component({
	selector: "app-sidebar",
	imports: [RouterModule, LogoComponent],
	templateUrl: "./sidebar.component.html"
})
export class SidebarComponent { }
