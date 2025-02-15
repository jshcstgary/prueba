import { Component, EventEmitter, input, Output } from "@angular/core";
import { RouterModule } from "@angular/router";

import { LogoComponent } from "@components";

@Component({
	selector: "app-navbar",
	imports: [RouterModule, LogoComponent],
	templateUrl: "./navbar.component.html"
})
export class NavbarComponent {
	public isLoading = input.required<boolean>();

	public names = input.required<string>();

	public surnames = input.required<string>();

	@Output() public readonly onSignOut = new EventEmitter<void>();

	public signOut(): void {
		this.onSignOut.emit();
	}
}
