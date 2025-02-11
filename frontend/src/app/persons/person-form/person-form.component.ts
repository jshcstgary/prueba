import { Component, ElementRef, EventEmitter, inject, Output, signal, viewChild } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";

import { HSOverlay } from "flyonui/flyonui";

@Component({
	selector: "app-person-form",
	standalone: false,
	templateUrl: "./person-form.component.html"
})
export class PersonFormComponent {
	public modal = viewChild<ElementRef<HTMLElement>>("createNewUserModal");

	public isFormValid = true;

	public isLoading = signal(false);

	private formBuilder = inject(FormBuilder);

	@Output() readonly onClose = new EventEmitter<void>();

	public myForm = this.formBuilder.group({
		names: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		surnames: ["", [Validators.required, Validators.minLength(1), Validators.maxLength(60)]],
		identification: ["", [Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
		birthDate: ["", [Validators.required]],
		user: this.formBuilder.group({
			username: ["", [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
			password: ["", [Validators.required, Validators.minLength(8)]]
		})
	});

	public onSubmit(): void {
		this.isLoading.set(!this.isLoading());

		setTimeout(() => {
			console.log("SAVING DATA");

			const createNewPersonModal = new HSOverlay(this.modal()!.nativeElement);

			createNewPersonModal.close();

			createNewPersonModal.on("close", () => {
				console.log("CLOSING MODAL");
				this.onClose.emit();
			});
		}, 2000);
	}
}
