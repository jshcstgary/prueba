import { Component, effect, ElementRef, EventEmitter, inject, Output, signal, viewChild } from "@angular/core";
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, Validators } from "@angular/forms";

import { format } from "date-fns";
import { HSOverlay } from "flyonui/flyonui";

import { PersonService } from "@services";

import { Person, PersonCreate, PersonUpdate, Role } from "@types";

import { openToast } from "@lib";

@Component({
	selector: "app-person-form",
	standalone: false,
	templateUrl: "./person-form.component.html"
})
export class PersonFormComponent {
	private formBuilder = inject(FormBuilder);

	private personService = inject(PersonService);

	private privateIsLoading = false;

	public personFormModal = viewChild<ElementRef<HTMLElement>>("personFormModal");

	public isLoading = signal(false);

	public idPerson = signal<number | null>(null);

	public roles = signal<Role[]>([]);

	public person = signal<Person | null>(null);

	public showErrors = signal(false);

	public myForm = this.formBuilder.group({
		names: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		surnames: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		identification: ["", [Validators.required, Validators.minLength(10), Validators.maxLength(10), Validators.pattern(/^[0-9]*$/), this.noFourRepeatedValidator]],
		birthDate: ["", [Validators.required, Validators.pattern(/^(19|20)\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$/)]],
		user: this.formBuilder.group({
			username: ["", [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*\d)(?=.*[A-Z])[A-Za-z\d]{8,}$/)]],
			password: [""],
			roles: this.formBuilder.array<FormGroup>([], Validators.required)
		})
	});

	@Output() public readonly onClose = new EventEmitter<boolean>();

	constructor() {
		effect(() => {
			if (this.idPerson() !== null && !this.privateIsLoading) {
				this.setFormValidationsForUpdate();
				this.getPersonById();
			}
		});
	}

	get rolesFormArray(): FormArray {
		return this.myForm.controls.user.controls.roles as FormArray;
	}

	private setFormValidationsForUpdate() {
		const passwordControl = this.myForm.controls.user.controls.password;

		if (passwordControl) {
			passwordControl.clearValidators();

			passwordControl.updateValueAndValidity();
		}
	}

	private getPersonById(): void {
		this.privateIsLoading = !this.privateIsLoading;
		this.isLoading.set(!this.isLoading());

		this.personService.getById(this.idPerson()!).subscribe({
			next: ({ data }) => {
				this.person.set(data!);

				this.fillRolesInForm(this.person()!.user.roles);

				this.myForm.patchValue({
					identification: this.person()!.identification,
					names: this.person()!.names,
					surnames: this.person()!.surnames,
					birthDate: format(this.person()!.birthDate, "yyyy-MM-dd"),
					user: {
						username: this.person()!.user.username,
						password: ""
					}
				});

				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;
			}
		});
	}

	private noFourRepeatedValidator(control: AbstractControl): ValidationErrors | null {
		const value = control.value;

		if (value && !/^(?!.*(\d)\1{3})\d+$/.test(value)) {
			return {
				repeated: true
			};
		}

		return null;
	}

	private fillRolesInForm(roles: Role[]): void {
		roles.forEach((role) => {
			this.rolesFormArray.push(this.formBuilder.group(role));
		});
	}

	private closeModal(reload = false): void {
		const deletePersonModal = new HSOverlay(this.personFormModal()!.nativeElement);

		deletePersonModal.close();

		this.onClose.emit(reload);
	}

	private createPerson(newPerson: PersonCreate[]): void {
		this.personService.create(newPerson).subscribe({
			next: ({ data }) => {
				const rowsChanged = data!;

				if (rowsChanged.rowsNotInserted > 0) {
					const personsWithProblems = rowsChanged.identificationsNotInserted[0][1];

					openToast(personsWithProblems, "error");

					return;
				}

				openToast("Registro insertado con éxito.", "success");

				this.onReset(true);

				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;
			}
		});
	}

	private updatePerson(personUpdated: PersonUpdate): void {
		this.personService.update(personUpdated).subscribe({
			next: () => {
				openToast("Registro actualizado con éxito.", "success");

				this.onReset(true);

				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;
			}
		});
	}

	public openModal(): void {
		const deletePersonModal = new HSOverlay(this.personFormModal()!.nativeElement);

		deletePersonModal.open();
	}

	public getNamesError(): string {
		if (this.myForm.controls.names.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public getSurnamesError(): string {
		if (this.myForm.controls.surnames.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public getIdentificationError(): string {
		if (this.myForm.controls.identification.hasError("required")) {
			return "Campo requerido";
		}

		if (this.myForm.controls.identification.hasError("minlength") || this.myForm.controls.identification.hasError("maxlength")) {
			return "Debe contener 10 dígitos";
		}

		if (this.myForm.controls.identification.hasError("pattern")) {
			return "Solo ingrese dígitos";
		}

		if (this.myForm.controls.identification.hasError("repeated")) {
			return "No repetir el mismo dígito 4 veces";
		}

		return "";
	}

	public getBirthDateError(): string {
		if (this.myForm.controls.birthDate.hasError("required")) {
			return "Campo requerido";
		}

		if (this.myForm.controls.birthDate.hasError("pattern")) {
			return "Formato de fecha YYYY-MM-DD (solo dígitos)";
		}

		return "";
	}

	public getRoleError(): string {
		if (this.myForm.controls.user.controls.username.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public getUsernameError(): string {
		if (this.myForm.controls.user.controls.username.hasError("required")) {
			return "Campo requerido";
		}

		if (this.myForm.controls.user.controls.username.hasError("pattern")) {
			return "Formato incorrecto";
		}

		return "";
	}

	public getPasswordError(): string {
		if (this.myForm.controls.user.controls.password.hasError("required")) {
			return "Campo requerido";
		}

		if (this.myForm.controls.user.controls.password.hasError("pattern")) {
			return "Formato incorrecto";
		}

		return "";
	}

	public addRoleToForm(event: Event) {
		const select = event.target as HTMLSelectElement;
		const idRoleSelected = Number(select.value);

		const role = this.roles().find((role) => role.id === idRoleSelected);

		const roles = this.rolesFormArray.value as Role[];

		if (role && !roles.some((e) => e.id === idRoleSelected)) {
			this.rolesFormArray.push(this.formBuilder.group(role));
		}

		select.value = "";
	}

	public removeRoleFromForm(index: number) {
		this.rolesFormArray.removeAt(index);
	}

	public onSubmit(): void {
		this.showErrors.set(false);

		if (this.myForm.invalid) {
			this.showErrors.set(this.myForm.invalid);

			openToast("Llene el formulario de forma correcta", "error");

			return;
		}

		this.privateIsLoading = !this.privateIsLoading;
		this.isLoading.set(!this.isLoading());

		if (this.idPerson() === null) {
			const newPerson: PersonCreate = {
				identification: this.myForm.value.identification!,
				names: this.myForm.value.names!,
				surnames: this.myForm.value.surnames!,
				birthDate: new Date(this.myForm.value.birthDate!),
				user: {
					username: this.myForm.value.user!.username!,
					password: this.myForm.value.user!.password!,
					roles: this.myForm.value.user!.roles!.map((role) => ({
						...role,
						roleOptions: []
					}))
				}
			};

			this.createPerson([newPerson]);
		} else {
			const personUpdated: PersonUpdate = {
				id: this.person()!.id,
				identification: this.myForm.value.identification!,
				names: this.myForm.value.names!,
				surnames: this.myForm.value.surnames!,
				birthDate: new Date(this.myForm.value.birthDate!),
				user: {
					id: this.person()!.user.id,
					username: this.myForm.value.user!.username!,
					password: this.myForm.value.user!.password!,
					mail: this.person()!.user.mail,
					sessionActive: this.person()!.user.sessionActive,
					status: this.person()!.user.status,
					roles: this.myForm.value.user!.roles!.map((role) => ({
						...role,
						roleOptions: []
					}))
				}
			};

			this.updatePerson(personUpdated);
		}
	}

	public onReset(reload = false): void {
		this.rolesFormArray.clear();

		this.myForm.reset({
			names: "",
			surnames: "",
			identification: "",
			birthDate: "",
			user: {
				roles: [],
				username: "",
				password: ""
			}
		});

		this.idPerson.set(null);
		this.roles.set([]);

		this.closeModal(reload);
	}
}
