import { Component, ElementRef, inject, OnInit, signal, viewChild } from "@angular/core";
import { forkJoin } from "rxjs";

import { parse } from "date-fns";
import * as XLSX from "xlsx";

import { ExcelService, PersonService, RoleService } from "@services";

import { PersonFormComponent } from "@persons";

import { ConfirmationModalComponent } from "@components";

import { Person, PersonCreate, Role } from "@types";

import { openToast } from "@lib";

import { Status } from "@constants";
import { HSOverlay } from "flyonui/flyonui";

type ExcelColumns = {
	identification: string;
	names: string;
	surnames: string;
	birthDate: string;
	username: string;
	password: string;
	idRoles: string;
};

@Component({
	selector: "app-person-list",
	standalone: false,
	templateUrl: "./person-list.component.html"
})
export class PersonListComponent implements OnInit {
	private personService = inject(PersonService);

	private roleService = inject(RoleService);

	private excelService = inject(ExcelService);

	public status = Status;

	public persons = signal<Person[]>([]);

	public roles = signal<Role[]>([]);

	public isLoading = signal<boolean>(false);

	public idPerson = signal<number>(0);

	public personsWithProblems = signal<string[][]>([]);

	public loadPersonExcelButton = viewChild<ElementRef>("loadPersonExcelInput");

	public notValidExcelWarn = viewChild<ElementRef<HTMLElement>>("notValidExcelWarn");

	public personFormModal = viewChild<PersonFormComponent>("personFormModal");

	public deletePersonModal = viewChild<ConfirmationModalComponent>("deletePersonModal");

	ngOnInit() {
		this.getPersons();
	}

	private getPersons(): void {
		this.isLoading.set(!this.isLoading());

		forkJoin([this.personService.getAll(), this.roleService.getAll(Status.Active)]).subscribe({
			next: ([personResponse, roleResponse]) => {
				this.persons.set(personResponse.data!);
				this.roles.set(roleResponse.data!);

				this.isLoading.set(!this.isLoading());
			},
			error: () => {
				this.persons.set([]);
				this.roles.set([]);

				this.isLoading.set(!this.isLoading());

				openToast("No se pudo obtener los registros.", "error");
			}
		});
	}

	public checkAdmin(roles: Role[]) {
		return roles.some((role) => role.name.toUpperCase() === "ADMINISTRADOR");
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

	public openPersonFormModal(idPerson: number | null): void {
		this.personFormModal()?.idPerson.set(idPerson);
		this.personFormModal()?.roles.set(this.roles());

		this.personFormModal()?.openModal();
	}

	public loadPersonExcel(): void {
		this.loadPersonExcelButton()!.nativeElement.click();
	}

	public onFileChange(event: Event): void {
		this.isLoading.set(!this.isLoading());

		let newPersons: PersonCreate[] = [];

		const input = event.target! as HTMLInputElement;
		const file = input.files![0];

		if (file === undefined || file === null) {
			openToast("Seleccione un archivo de Excel.", "error");

			this.isLoading.set(!this.isLoading());

			return;
		}

		const reader: FileReader = new FileReader();

		reader.onload = (e: ProgressEvent<FileReader>) => {
			const binaryString: string = e.target!.result as string;
			const workbook: XLSX.WorkBook = XLSX.read(binaryString, {
				type: "binary"
			});
			const sheetName: string = workbook.SheetNames[0];
			const worksheet: XLSX.WorkSheet = workbook.Sheets[sheetName];

			const jsonData: ExcelColumns[] = (XLSX.utils.sheet_to_json(worksheet) as ExcelColumns[]).map((excelData) => ({
				identification: excelData.identification.toString(),
				names: excelData.names.toString(),
				surnames: excelData.surnames.toString(),
				birthDate: excelData.birthDate.toString(),
				username: excelData.username.toString(),
				password: excelData.password.toString(),
				idRoles: excelData.idRoles.toString()
			}));

			const identificationRegExp = new RegExp(/^[0-9]*$/);
			const identificationNotFurConsecutive = new RegExp(/^(?!.*(\d)\1{3})\d+$/);
			const dateFormat = new RegExp(/^(19|20)\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$/);
			const usernameFormat = new RegExp(/^(?=.*\d)(?=.*[A-Z])[A-Za-z\d]{8,}$/);
			const passwordFormat = new RegExp(/^(?=.*[A-Z])(?=.*[\W_])(?=\S).*/);

			jsonData.forEach((data) => {
				const textWarn: string[] = [];

				if (data.identification === "" || data.identification === undefined || data.identification === null || data.identification.length !== 10 || !identificationRegExp.test(data.identification) || !identificationNotFurConsecutive.test(data.identification)) {
					if (textWarn.length === 0) {
						textWarn.push(`${data.identification}:`);
					}

					textWarn.push("Debe ingresar un número de identificación de 10 dígitos, solo números, sin tener 4 dígitos repetidos consecutivos.");
				}

				if (data.names === "" || data.names === undefined || data.names === null) {
					if (textWarn.length === 0) {
						textWarn.push(`${data.identification}:`);
					}

					textWarn.push("Debe ingresar un nommbre.");
				}

				if (data.surnames === "" || data.surnames === undefined || data.surnames === null || !data.surnames.includes(" ")) {
					if (textWarn.length === 0) {
						textWarn.push(`${data.identification}:`);
					}

					textWarn.push("Debe ingresar los dos apellido.");
				}

				if (data.birthDate === "" || data.birthDate === undefined || data.birthDate === null || !dateFormat.test(data.birthDate)) {
					if (textWarn.length === 0) {
						textWarn.push(`${data.identification}:`);
					}

					textWarn.push("Debe ingresar la fecha en el formato YYYY-MM-DD.");
				}

				if (data.username === "" || data.username === undefined || data.username === null || !usernameFormat.test(data.username)) {
					if (textWarn.length === 0) {
						textWarn.push(`${data.identification}:`);
					}

					textWarn.push("Debe ingresar un nombre de usuario, sin caracteres especiales, al menos un número, al menos una letra mayúscula y mínimo 8 caracteres.");
				}

				if (data.password === "" || data.password === undefined || data.password === null || !passwordFormat.test(data.password)) {
					if (textWarn.length === 0) {
						textWarn.push(`${data.identification}:`);
					}

					textWarn.push("Debe ingresar una contraseña, sin espacios, al menos un caracter especial, al menos un número, al menos una letra mayúscula y mínimo 8 caracteres.");
				}

				if (data.idRoles === "" || data.idRoles === undefined || data.idRoles === null) {
					if (textWarn.length === 0) {
						textWarn.push(`${data.identification}:`);
					}

					textWarn.push("Debe ingresar IDs de roles separados por coma.");
				}

				let idsNotFound = "";

				data.idRoles.split(",").forEach((idRole) => {
					const roleFound = this.roles().find((role) => role.id === Number(idRole));

					if (roleFound === undefined || roleFound === null) {
						idsNotFound += `${idRole},`;
					}
				});

				if (idsNotFound !== "") {
					if (textWarn.length === 0) {
						textWarn.push(`${data.identification}:`);
					}

					textWarn.push(` Los siguientes IDs: ${idsNotFound} no son de roles existentes.`);
				}

				if (textWarn.length > 0) {
					this.personsWithProblems().push(textWarn);
				}
			});

			if (this.personsWithProblems().length > 0) {
				openToast("No todos los registros son válidos.", "error");

				const notValidPersons = new HSOverlay(this.notValidExcelWarn()!.nativeElement);

				notValidPersons.open();

				this.isLoading.set(!this.isLoading());

				return;
			}

			newPersons = jsonData.map((data) => ({
				identification: data.identification,
				names: data.names,
				surnames: data.surnames,
				birthDate: parse(data.birthDate, "yyyy-MM-dd", new Date()),
				user: {
					username: data.username,
					password: data.password,
					roles: data.idRoles.split(",").map((idRole) => this.roles().find((role) => role.id === Number(idRole)))!
				}
			})) as PersonCreate[];

			this.personService.create(newPersons).subscribe({
				next: ({ data }) => {
					const rowsChanged = data!;

					if (rowsChanged.rowsNotInserted > 0) {
						this.personsWithProblems.set(rowsChanged.identificationsNotInserted);

						openToast("No todos los registros se insertaron.", "warning");

						const notValidPersons = new HSOverlay(this.notValidExcelWarn()!.nativeElement);

						notValidPersons.open();
					}

					this.isLoading.set(!this.isLoading());

					this.getPersons();
				},
				error: (err) => {
					openToast(err.error.errorMessage, "error");

					this.isLoading.set(!this.isLoading());
				}
			});
		};

		reader.readAsArrayBuffer(file);
	}

	public downloadTemplate(): void {
		const headers = [
			["identification", "names", "surnames", "birthDate", "username", "password", "idRoles"],
			["xxxxxxxxxx", "", "", "YYYY-MM-DD", "", "", "IDs separados por coma"]
		];

		this.excelService.exportExcelTemplate("PlantillaUsuarios", headers);
	}

	public closeErrorPersonExcel(): void {
		const notValidPersons = new HSOverlay(this.notValidExcelWarn()!.nativeElement);

		notValidPersons.close();
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
}
