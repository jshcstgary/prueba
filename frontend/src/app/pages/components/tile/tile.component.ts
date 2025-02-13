import { Component, input } from "@angular/core";

@Component({
	selector: "app-tile",
	standalone: false,
	templateUrl: "./tile.component.html",
	host: {
		style: "display: block;"
	}
})
export class TileComponent {
	public amount = input.required<number>();
	public label = input.required<string>();

	public colorFrom = input<string>("from-yellow-500");
	public colorTo = input<string>("to-yellow-300");
}
