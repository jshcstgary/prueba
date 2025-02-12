import { Component, effect, ElementRef, EventEmitter, HostBinding, inject, Output, signal, viewChild } from "@angular/core";
import { AbstractControl, FormBuilder, ValidationErrors, Validators } from "@angular/forms";

import { HSOverlay } from "flyonui/flyonui";
import { format } from "date-fns";

import { PersonService } from "@services";

import { PersonCreate } from "@types";

import { openToast } from "@lib";

@Component({
	selector: "app-person-form",
	standalone: false,
	templateUrl: "./person-form.component.html"
})
export class PersonFormComponent {
	// public modal = viewChild<ElementRef<HTMLElement>>("personFormModal");
	public formPersonModal = viewChild<ElementRef<HTMLElement>>("personFormModal");

	public isFormValid = true;

	public isLoading = signal(false);

	public idPerson = signal<number | null>(null);

	public showErrors = signal(false);

	private formBuilder = inject(FormBuilder);

	private personService = inject(PersonService);

	@Output() readonly onClose = new EventEmitter<boolean>();

	constructor() {
		effect(() => {
			if (this.idPerson() !== null) {
				this.getPersonById();
			}
		});
	}

	public myForm = this.formBuilder.group({
		names: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		surnames: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		identification: ["", [Validators.required, Validators.minLength(10), Validators.maxLength(10), Validators.pattern("^[0-9]*$"), this.noFourRepeated]],
		birthDate: ["", [Validators.required, Validators.pattern(/^(19|20)\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$/)]],
		user: this.formBuilder.group({
			username: ["", [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*\d)(?=.*[A-Z])[A-Za-z\d]{8,}$/)]],
			password: ["", [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Z])(?=.*[\W_])(?=\S).*/)]]
		})
	});

	public open(): void {
		const deletePersonModal = new HSOverlay(this.formPersonModal()!.nativeElement);

		deletePersonModal.open();
	}

	private noFourRepeated(control: AbstractControl): ValidationErrors | null {
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

	public getPersonById(): void {
		this.personService.getById(this.idPerson()!).subscribe({
			next: ({ data }) => {
				const {
					identification,
					names,
					surnames,
					birthDate,
					user: { username }
				} = data!;

				this.myForm.setValue({
					identification,
					names,
					surnames,
					birthDate: format(birthDate, "yyyy-MM-dd"),
					user: {
						username,
						password: ""
					}
				});
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");
			}
		});
	}

	public onSubmit(): void {
		this.showErrors.set(false);

		if (this.myForm.invalid) {
			this.showErrors.set(this.myForm.invalid);

			openToast("Llene el formulario de forma correcta", "error");

			return;
		}

		this.isLoading.set(!this.isLoading());

		const newPerson: PersonCreate = {
			identification: this.myForm.value.identification!,
			names: this.myForm.value.names!,
			surnames: this.myForm.value.surnames!,
			birthDate: new Date(this.myForm.value.birthDate!),
			user: {
				username: this.myForm.value.user!.username!,
				password: this.myForm.value.user!.password!
			}
		};

		this.personService.create(newPerson).subscribe({
			next: () => {
				this.isLoading.set(!this.isLoading());

				this.close(true);
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
			}
		});
	}

	public close(reload = false): void {
		const deletePersonModal = new HSOverlay(this.formPersonModal()!.nativeElement);

		deletePersonModal.close();

		this.onClose.emit(reload);
	}
}
