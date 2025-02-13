import { Component, effect, ElementRef, EventEmitter, inject, Output, signal, viewChild } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { openToast } from "@lib";
import { RoleOptionService } from "@services";
import { RoleOptionCreate } from "@types";

import { HSOverlay } from "flyonui/flyonui";

@Component({
	selector: "app-role-option-form",
	standalone: false,
	templateUrl: "./role-option-form.component.html"
})
export class RoleOptionFormComponent {
	public roleOptionFormModal = viewChild<ElementRef<HTMLElement>>("roleOptionFormModal");

	public isLoading = signal(false);

	public idRoleOption = signal<number | null>(null);

	public showErrors = signal(false);

	private formBuilder = inject(FormBuilder);

	private roleOptionService = inject(RoleOptionService);

	@Output() readonly onClose = new EventEmitter<boolean>();

	public myForm = this.formBuilder.group({
		name: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]]
	});

	constructor() {
		effect(() => {
			if (this.idRoleOption() !== null) {
				this.getRoleOptionById();
			}
		});
	}

	public open(): void {
		const deletePersonModal = new HSOverlay(this.roleOptionFormModal()!.nativeElement);

		deletePersonModal.open();
	}

	public getNameError(): string {
		if (this.myForm.controls.name.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public getRoleOptionById(): void {
		this.roleOptionService.getById(this.idRoleOption()!).subscribe({
			next: ({ data }) => {
				const { name } = data!;

				this.myForm.setValue({
					name
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

		const newRoleOption: RoleOptionCreate = {
			name: this.myForm.value.name!
		};

		this.roleOptionService.create(newRoleOption).subscribe({
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
		const deletePersonModal = new HSOverlay(this.roleOptionFormModal()!.nativeElement);

		deletePersonModal.close();

		this.onClose.emit(reload);
	}
}
