import { Component, inject, OnInit, signal } from "@angular/core";

import { PersonService } from "@services";

import { ColorTile, Count, PersonCount } from "@types";

import { openToast } from "@lib";

import { colorTiles } from "@data";

@Component({
	selector: "app-dashboard",
	standalone: false,
	templateUrl: "./dashboard.component.html"
})
export class DashboardComponent implements OnInit {
	public isLoading = signal(false);

	public personCount = signal<Count[]>([]);

	private personService = inject(PersonService);

	ngOnInit() {
		this.getCount();
	}

	private getCount(): void {
		this.isLoading.set(!this.isLoading());

		this.personService.getCount().subscribe({
			next: (response) => {
				const personCount = response.data!;

				const result = this.combineArrays(personCount, colorTiles);

				this.personCount.set(result);

				this.isLoading.set(!this.isLoading());
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
			}
		});
	}

	private combineArrays(firstArray: PersonCount[], secondArray: ColorTile[]): Count[] {
		let secondArrayIndex = 0;

		const result: Count[] = firstArray.map(fa => {
			const pair: Count = {
				label: fa.label,
				amount: fa.amount,
				colorFrom: secondArray[secondArrayIndex].colorFrom,
				colorTo: secondArray[secondArrayIndex].colorTo
			};

			secondArrayIndex = (secondArrayIndex + 1) % secondArray.length;

			return pair;
		});

		return result;
	}
}
