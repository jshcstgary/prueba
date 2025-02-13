import { Component, effect, ElementRef, EventEmitter, inject, Output, signal, viewChild } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";

import { HSOverlay } from "flyonui/flyonui";

import { RoleService } from "@services";

import { RoleCreate, RoleOption } from "@types";

import { openToast } from "@lib";

@Component({
	selector: "app-role-form",
	standalone: false,
	templateUrl: "./role-form.component.html"
})
export class RoleFormComponent {
	public roleFormModal = viewChild<ElementRef<HTMLElement>>("roleFormModal");

	public isLoading = signal(false);

	public idRole = signal<number | null>(null);

	public roleOptions = signal<RoleOption[]>([]);

	public showErrors = signal(false);

	private formBuilder = inject(FormBuilder);

	private roleService = inject(RoleService);

	@Output() readonly onClose = new EventEmitter<boolean>();

	public myForm = this.formBuilder.group({
		name: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		roleOptions: this.formBuilder.array<FormGroup>([], Validators.required)
	});

	constructor() {
		effect(() => {
			if (this.idRole() !== null) {
				this.getRoleById();
			}
		});
	}

	get roleOptionsFormArray(): FormArray {
		return this.myForm.controls.roleOptions as FormArray;
	}

	public open(): void {
		const deletePersonModal = new HSOverlay(this.roleFormModal()!.nativeElement);

		deletePersonModal.open();
	}

	public getRoleById(): void {
		this.roleService.getById(this.idRole()!).subscribe({
			next: ({ data }) => {
				const { name } = data!;

				this.myForm.setValue({
					name,
					roleOptions: []
				});
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");
			}
		});
	}

	public addRoleOptionToForm(event: Event) {
		const select = event.target as HTMLSelectElement;
		const idRoleOptionSelected = Number(select.value);

		const roleOption = this.roleOptions().find((roleOption) => roleOption.id === idRoleOptionSelected);

		const roleOptions = this.roleOptionsFormArray.value as RoleOption[];

		if (roleOption && !roleOptions.some((e) => e.id === idRoleOptionSelected)) {
			this.roleOptionsFormArray.push(this.formBuilder.group(roleOption));
		}

		select.value = "";
	}

	public removeRoleOptionFromForm(index: number) {
		this.roleOptionsFormArray.removeAt(index);
	}

	public getNameError(): string {
		if (this.myForm.controls.name.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public getRoleOptionsError(): string {
		if (this.myForm.controls.roleOptions.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public onSubmit(): void {
		console.log(this.myForm.controls.roleOptions);
		this.showErrors.set(false);

		if (this.myForm.invalid) {
			this.showErrors.set(this.myForm.invalid);

			openToast("Llene el formulario de forma correcta", "error");

			return;
		}

		this.isLoading.set(!this.isLoading());

		const newRole: RoleCreate = {
			name: this.myForm.value.name!,
			roleOptions: this.myForm.value.roleOptions!
		};

		this.roleService.create(newRole).subscribe({
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
		this.roleOptionsFormArray.clear();

		this.myForm.patchValue({
			name: ""
		});

		const deletePersonModal = new HSOverlay(this.roleFormModal()!.nativeElement);

		deletePersonModal.close();

		this.onClose.emit(reload);
	}
}
