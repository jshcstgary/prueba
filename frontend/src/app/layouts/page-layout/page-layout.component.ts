import { Component, inject, signal } from "@angular/core";
import { ActivatedRoute, NavigationEnd, Router, RouterModule } from "@angular/router";
import { filter, map, mergeMap } from "rxjs";
import { Title } from "@angular/platform-browser";

import { AuthService, PersonService } from "@services";

import { NavbarComponent, SidebarComponent } from "@components";

import { LocalStorageKeys } from "@constants";

import { Person, Role, RoleOption } from "@types";

import { openToast } from "@lib";

@Component({
	selector: "app-page-layout",
	imports: [RouterModule, NavbarComponent, SidebarComponent],
	templateUrl: "./page-layout.component.html"
})
export class PageLayoutComponent {
	private personService = inject(PersonService);

	private authService = inject(AuthService);

	private route = inject(ActivatedRoute);

	private router = inject(Router);

	private titleService = inject(Title);

	public title = signal("");

	public isLoading = signal(false);

	public names = signal("");

	public surnames = signal("");

	public roleOptions = signal<RoleOption[]>([]);
	constructor() {
		this.router.events
			.pipe(
				filter((event) => event instanceof NavigationEnd),
				map(() => this.route),
				map((route) => {
					while (route.firstChild) {
						route = route.firstChild;
					}

					return route;
				}),
				filter((route) => route.outlet === "primary"),
				mergeMap((route) => route.data)
			)
			.subscribe((data) => {
				this.titleService.setTitle(data["title"]);

				this.title.set(data["title"]);
			});

		const isAuthenticated: boolean | null = Boolean(localStorage.getItem(LocalStorageKeys.isAuthenticated));

		if (isAuthenticated !== null && isAuthenticated) {
			this.getData();
		}
	}

	private getData(): void {
		this.isLoading.set(!this.isLoading());

		if (this.authService.authenticatedPerson() !== null && this.authService.authenticatedIdRole() !== 0 && this.authService.authenticatedRoleOptions().length > 0) {
			this.settingData();

			this.isLoading.set(!this.isLoading());

			return;
		}

		this.personService.getById(Number(localStorage.getItem(LocalStorageKeys.idPerson)!)).subscribe({
			next: ({ data }) => {
				const person: Person = data!;
				const idRole = Number(localStorage.getItem(LocalStorageKeys.idRole)!);

				const roleOptions = person.user.roles.find((role: Role) => role.id === idRole)!.roleOptions;

				this.authService.authenticatedPerson.set(person);
				this.authService.authenticatedIdRole.set(idRole);
				this.authService.authenticatedRoleOptions.set(roleOptions);

				this.settingData();

				this.isLoading.set(!this.isLoading());
			}
		});
	}

	private settingData(): void {
		this.roleOptions.set(this.authService.authenticatedRoleOptions());
		this.names.set(this.authService.authenticatedPerson()!.names);
		this.surnames.set(this.authService.authenticatedPerson()!.surnames);
	}

	public onSignOut(): void {
		localStorage.removeItem(LocalStorageKeys.idPerson);
		localStorage.removeItem(LocalStorageKeys.idRole);
		localStorage.removeItem(LocalStorageKeys.isAuthenticated);

		this.authService.authenticatedIdRole.set(0);
		this.authService.authenticatedPerson.set(null);
		this.authService.authenticatedRoleOptions.set([]);

		this.router.navigate(["/auth"]);
		// this.isLoading.set(!this.isLoading());

		// this.authService.signOut().subscribe({
		// 	next: () => {
		// 		this.isLoading.set(!this.isLoading());

		// 		this.router.navigate(["/auth"]);
		// 	},
		// 	error: () => {
		// 		openToast("No se pudo cerrar sesión", "error");

		// 		this.isLoading.set(!this.isLoading());
		// 	}
		// });
	}
}
