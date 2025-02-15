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
	private formBuilder = inject(FormBuilder);

	private roleOptionService = inject(RoleOptionService);

	private privateIsLoading = false;

	public roleOptionFormModal = viewChild<ElementRef<HTMLElement>>("roleOptionFormModal");

	public isLoading = signal(false);

	public idRoleOption = signal<number | null>(null);

	public showErrors = signal(false);

	public myForm = this.formBuilder.group({
		name: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		link: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(20)]]
	});

	@Output() public readonly onClose = new EventEmitter<boolean>();

	constructor() {
		effect(() => {
			if (this.idRoleOption() !== null && !this.privateIsLoading) {
				this.getRoleOptionById();
			}
		});
	}

	private closeModal(reload = false): void {
		const deletePersonModal = new HSOverlay(this.roleOptionFormModal()!.nativeElement);

		deletePersonModal.close();

		this.onClose.emit(reload);
	}

	public openModal(): void {
		const deletePersonModal = new HSOverlay(this.roleOptionFormModal()!.nativeElement);

		deletePersonModal.open();
	}

	public getNameError(): string {
		if (this.myForm.controls.name.hasError("required")) {
			return "Campo requerido";
		}

		if (this.myForm.controls.name.hasError("maxlength")) {
			return "Nombre muy extenso";
		}

		return "";
	}

	public getLinkError(): string {
		if (this.myForm.controls.link.hasError("required")) {
			return "Campo requerido";
		}

		if (this.myForm.controls.link.hasError("maxlength")) {
			return "Enlace muy extenso";
		}

		return "";
	}

	public getRoleOptionById(): void {
		this.privateIsLoading = !this.privateIsLoading;
		this.isLoading.set(!this.isLoading());

		this.roleOptionService.getById(this.idRoleOption()!).subscribe({
			next: ({ data }) => {
				const { name, link } = data!;

				this.myForm.setValue({
					name,
					link
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

	public onSubmit(): void {
		this.showErrors.set(false);

		if (this.myForm.invalid) {
			this.showErrors.set(this.myForm.invalid);

			openToast("Llene el formulario de forma correcta", "error");

			return;
		}

		this.privateIsLoading = !this.privateIsLoading;
		this.isLoading.set(!this.isLoading());

		const newRoleOption: RoleOptionCreate = {
			name: this.myForm.value.name!,
			link: this.myForm.value.link!
		};

		this.roleOptionService.create(newRoleOption).subscribe({
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

	public onReset(reload = false): void {
		this.myForm.reset({
			name: "",
			link: ""
		});

		this.idRoleOption.set(null);

		this.closeModal(reload);
	}
}
