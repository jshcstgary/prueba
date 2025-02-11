import { Component, inject, OnInit, signal } from "@angular/core";
import { PersonService } from "@services";

interface User {
	identification: string;
	names: string;
	surnames: string;
	username: string;
	email: string;
	sessionActive: boolean;
	status: string;
}

@Component({
	selector: "app-list",
	standalone: false,
	templateUrl: "./list.component.html"
})
export class ListComponent implements OnInit {
	users = signal<User[]>([
		// {
		// 	identification: "0987654321",
		// 	names: "Miguel Joshua",
		// 	surnames: "Castillo Riofrío",
		// 	username: "joshcast",
		// 	email: "josh.cast@mail.com",
		// 	sessionActive: true,
		// 	status: "ACTIVE"
		// },
		// {
		// 	identification: "1234567890",
		// 	names: "Valentina Alexandra",
		// 	surnames: "Vidal Navarrete",
		// 	username: "valeale",
		// 	email: "vale.ale@mail.com",
		// 	sessionActive: false,
		// 	status: "LOCK"
		// },
		// {
		// 	identification: "1092837465",
		// 	names: "Britanny Aleska",
		// 	surnames: "Villamar Ventura",
		// 	username: "alevi",
		// 	email: "ale.vi@mail.com",
		// 	sessionActive: false,
		// 	status: "DELETE"
		// }
	]);

	private personService = inject(PersonService);

	ngOnInit() {
		this.personService.getAll().subscribe({
			next: (response) => {
				console.log(response);
			}
		});
	}

	close() {
		console.log("GETTING USERS");
	}
}
