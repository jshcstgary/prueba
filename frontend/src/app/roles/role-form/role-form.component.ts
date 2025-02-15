import { Component, effect, ElementRef, EventEmitter, inject, Output, signal, viewChild } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";

import { HSOverlay } from "flyonui/flyonui";

import { RoleService } from "@services";

import { Role, RoleCreate, RoleOption } from "@types";

import { openToast } from "@lib";

@Component({
	selector: "app-role-form",
	standalone: false,
	templateUrl: "./role-form.component.html"
})
export class RoleFormComponent {
	private formBuilder = inject(FormBuilder);

	private roleService = inject(RoleService);

	private privateIsLoading = false;

	public roleFormModal = viewChild<ElementRef<HTMLElement>>("roleFormModal");

	public isLoading = signal(false);

	public idRole = signal<number | null>(null);

	public roleOptions = signal<RoleOption[]>([]);

	public role = signal<Role | null>(null);

	public showErrors = signal(false);

	public myForm = this.formBuilder.group({
		name: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		roleOptions: this.formBuilder.array<FormGroup>([], Validators.required)
	});

	@Output() public readonly onClose = new EventEmitter<boolean>();

	constructor() {
		effect(() => {
			if (this.idRole() !== null && !this.privateIsLoading) {
				this.getRoleById();
			}
		});
	}

	get roleOptionsFormArray(): FormArray {
		return this.myForm.controls.roleOptions as FormArray;
	}

	private fillRoleOptionsInForm(roleOptions: RoleOption[]): void {
		roleOptions.forEach((roleOption) => {
			this.roleOptionsFormArray.push(this.formBuilder.group(roleOption));
		});
	}

	private closeModal(reload = false): void {
		const deletePersonModal = new HSOverlay(this.roleFormModal()!.nativeElement);

		deletePersonModal.close();

		this.onClose.emit(reload);
	}

	public openModal(): void {
		const deletePersonModal = new HSOverlay(this.roleFormModal()!.nativeElement);

		deletePersonModal.open();
	}

	private getRoleById(): void {
		this.privateIsLoading = !this.privateIsLoading;
		this.isLoading.set(!this.isLoading());

		this.roleService.getById(this.idRole()!).subscribe({
			next: ({ data }) => {
				this.role.set(data!);

				this.myForm.setValue({
					name: this.role()!.name,
					roleOptions: []
				});

				this.fillRoleOptionsInForm(this.role()!.roleOptions);

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

	private createRole(newRole: RoleCreate): void {
		this.roleService.create(newRole).subscribe({
			next: () => {
				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;

				this.closeModal(true);
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;
			}
		});
	}

	private updateRole(newRole: Role): void {
		this.roleService.update(newRole).subscribe({
			next: () => {
				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;

				this.closeModal(true);
			},
			error: (err) => {
				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
				this.privateIsLoading = !this.privateIsLoading;
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

	public removeRoleOptionFromForm(index: number): void {
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
		this.showErrors.set(false);

		if (this.myForm.invalid) {
			this.showErrors.set(this.myForm.invalid);

			openToast("Llene el formulario de forma correcta", "error");

			return;
		}

		this.privateIsLoading = !this.privateIsLoading;
		this.isLoading.set(!this.isLoading());

		if (this.idRole() === null) {
			const newRole: RoleCreate = {
				name: this.myForm.value.name!,
				roleOptions: this.myForm.value.roleOptions!
			};

			this.createRole(newRole);
		} else {
			const roleUpdated: Role = {
				id: this.role()!.id,
				name: this.myForm.value.name!,
				roleOptions: this.myForm.value.roleOptions!,
				status: this.role()!.status,
			};

			this.updateRole(roleUpdated);
		}
	}

	public onReset(reload = false): void {
		this.roleOptionsFormArray.clear();

		this.myForm.reset({
			name: "",
			roleOptions: []
		});

		this.idRole.set(null);
		this.roleOptions.set([]);

		this.closeModal(reload);
	}
}
