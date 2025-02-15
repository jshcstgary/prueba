import { Component, inject, OnInit, signal } from "@angular/core";
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidationErrors, Validators } from "@angular/forms";

import { AuthService, PersonService } from "@services";

import { Person, PersonUpdate, Role } from "@types";

import { openToast } from "@lib";
import { format } from "date-fns";
import { Router } from "@angular/router";

@Component({
	selector: "app-person-auth-form",
	standalone: false,
	templateUrl: "./person-auth-form.component.html"
})
export class PersonAuthFormComponent implements OnInit {
	private router = inject(Router);

	private authService = inject(AuthService);

	private personService = inject(PersonService);

	private formBuilder = inject(FormBuilder);

	public isLoading = signal(false);

	public showErrors = signal(false);

	public person = signal<Person>(this.authService.authenticatedPerson()!);

	public roles = signal<Role[]>([]);

	public myForm = this.formBuilder.group({
		names: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		surnames: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		identification: ["", [Validators.required, Validators.minLength(10), Validators.maxLength(10), Validators.pattern("^[0-9]*$"), this.noFourRepeatedValidator]],
		birthDate: ["", [Validators.required, Validators.pattern(/^(19|20)\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$/)]],
		user: this.formBuilder.group({
			username: ["", [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*\d)(?=.*[A-Z])[A-Za-z\d]{8,}$/)]],
			mail: [
				{
					value: "",
					disabled: true
				},
				[Validators.required]
			],
			password: ["", [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Z])(?=.*[\W_])(?=\S).*/)]],
			roles: this.formBuilder.array<FormGroup>([], Validators.required)
		})
	});

	get rolesFormArray(): FormArray {
		return this.myForm.controls.user.controls.roles as FormArray;
	}

	ngOnInit() {
		this.fillForm();
	}

	private fillForm(): void {
		const {
			identification,
			names,
			surnames,
			birthDate,
			user: { username, mail, roles }
		} = this.person();

		this.myForm.setValue({
			identification,
			names,
			surnames,
			birthDate: format(birthDate, "yyyy-MM-dd"),
			user: {
				username,
				mail,
				password: "",
				roles: []
			}
		});

		this.fillRolesInForm(roles);
	}

	private fillRolesInForm(roles: Role[]): void {
		roles.forEach((role) => {
			this.rolesFormArray.push(this.formBuilder.group(role));
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

		const role = this.roles().find((role: Role) => role.id === idRoleSelected);

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

		this.isLoading.set(!this.isLoading());

		const personUpdated: PersonUpdate = {
			id: this.authService.authenticatedPerson()!.user.id,
			identification: this.myForm.value.identification!,
			names: this.myForm.value.names!,
			surnames: this.myForm.value.surnames!,
			birthDate: new Date(this.myForm.value.birthDate!),
			user: {
				id: this.authService.authenticatedPerson()!.id,
				username: this.myForm.value.user!.username!,
				password: this.myForm.value.user!.password!,
				mail: this.authService.authenticatedPerson()!.user.mail,
				sessionActive: this.authService.authenticatedPerson()!.user.sessionActive,
				status: this.authService.authenticatedPerson()!.user.status,
				roles: this.myForm.value.user!.roles!
			}
		};

		this.personService.update(personUpdated).subscribe({
			next: () => {
				this.isLoading.set(!this.isLoading());

				this.onReset();
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
			}
		});
	}

	public onReset(): void {
		this.rolesFormArray.clear();

		this.myForm.reset();

		this.router.navigate(["/"]);
	}
}
