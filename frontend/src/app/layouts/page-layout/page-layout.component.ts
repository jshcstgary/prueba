import { Component, inject, signal } from "@angular/core";
import { ActivatedRoute, NavigationEnd, Router, RouterModule } from "@angular/router";
import { Title } from "@angular/platform-browser";

import { filter, map, mergeMap } from "rxjs";

import { NavbarComponent, SidebarComponent } from "@components";

@Component({
	selector: "app-page-layout",
	imports: [RouterModule, NavbarComponent, SidebarComponent],
	templateUrl: "./page-layout.component.html"
})
export class PageLayoutComponent {
	private router = inject(Router);
	private route = inject(ActivatedRoute);
	private titleService = inject(Title);

	public title = signal("");

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
	}
}
