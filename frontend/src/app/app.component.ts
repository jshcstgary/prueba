import { Component, OnInit } from "@angular/core";
import { NavigationEnd, Router } from "@angular/router";

import { IStaticMethods } from "flyonui/flyonui";

declare global {
	interface Window {
		HSStaticMethods: IStaticMethods;
	}
}

@Component({
	selector: "app-root",
	templateUrl: "./app.component.html",
	standalone: false
})
export class AppComponent implements OnInit {
	title = "prueba-viamatica-frontend";

	constructor(private router: Router) {}

	ngOnInit() {
		// eslint-disable-next-line @typescript-eslint/no-explicit-any
		this.router.events.subscribe((event: any) => {
			if (event instanceof NavigationEnd) {
				setTimeout(() => {
					window.HSStaticMethods.autoInit();
				}, 100);
			}
		});
	}
}
