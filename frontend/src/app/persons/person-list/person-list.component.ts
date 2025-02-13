import { Component, inject, OnInit, signal, viewChild } from "@angular/core";

import { PersonService } from "@services";

import { PersonFormComponent } from "@persons";

import { ConfirmationModalComponent } from "@components";

import { Person } from "@types";

import { openToast } from "@lib";

import { Status } from "@constants";

@Component({
	selector: "app-person-list",
	standalone: false,
	templateUrl: "./person-list.component.html"
})
export class PersonListComponent implements OnInit {
	public status = Status;

	public persons = signal<Person[]>([]);

	public isLoading = signal<boolean>(false);

	public idPerson = signal<number>(0);

	// public personFormModal = viewChild<ElementRef<HTMLElement>>("personFormModal");
	public personFormModal = viewChild<PersonFormComponent>("personFormModal");

	public deletePersonModal = viewChild<ConfirmationModalComponent>("deletePersonModal");

	private personService = inject(PersonService);

	ngOnInit() {
		this.getPersons();
	}

	private getPersons(): void {
		this.isLoading.set(!this.isLoading());

		this.personService.getAll().subscribe({
			next: (response) => {
				this.persons.set(response.data!);

				this.isLoading.set(!this.isLoading());
			},
			error: () => {
				this.persons.set([]);

				this.isLoading.set(!this.isLoading());

				openToast("No se pudo obtener los registros.", "error");
			}
		});
	}

	public openPersonFormModal(idPerson: number | null): void {
		this.personFormModal()?.idPerson.set(idPerson);

		this.personFormModal()?.open();
	}

	public openDeletePersonModal(idPerson: number): void {
		this.idPerson.set(idPerson);

		this.deletePersonModal()?.type.set("error");

		this.deletePersonModal()?.open();
	}

	public onClosePersonForm(reload: boolean): void {
		if (reload) {
			this.getPersons();
		}
	}

	public onConfirmation(isConfirmed: boolean): void {
		if (!isConfirmed) {
			this.deletePersonModal()?.close();

			return;
		}

		this.deletePerson();
	}

	private deletePerson(): void {
		this.deletePersonModal()?.isLoading.set(!this.deletePersonModal()?.isLoading());

		this.personService.delete(this.idPerson()).subscribe({
			next: () => {
				this.deletePersonModal()?.isLoading.set(!this.deletePersonModal()?.isLoading());
				this.deletePersonModal()?.close();

				openToast("Registro eliminado.", "success");

				this.getPersons();
			},
			error: (err) => {
				this.deletePersonModal()?.isLoading.set(!this.deletePersonModal()?.isLoading());
				this.deletePersonModal()?.close();

				openToast(err.error.errorMessage, "error");

				this.getPersons();
			}
		});
	}
}
