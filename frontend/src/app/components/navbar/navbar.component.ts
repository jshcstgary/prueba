import { Component, EventEmitter, Output, signal } from "@angular/core";
import { RouterModule } from "@angular/router";

import { LogoComponent } from "@components";

@Component({
	selector: "app-navbar",
	imports: [RouterModule, LogoComponent],
	templateUrl: "./navbar.component.html"
})
export class NavbarComponent {
	private isDrawerOpen = signal(false);

	@Output() readonly titleChanged = new EventEmitter<boolean>();

	public openDrawer(): void {
		this.isDrawerOpen.set(!this.isDrawerOpen());
	}
}
