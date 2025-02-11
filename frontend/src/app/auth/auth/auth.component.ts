import { Component, inject } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";

@Component({
	selector: "app-auth",
	standalone: false,
	templateUrl: "./auth.component.html"
})
export class AuthComponent {
	public isFormValid = true;

	private formBuilder = inject(FormBuilder);

	public myForm = this.formBuilder.group({
		emailUsername: ["", [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
		password: ["", [Validators.required, Validators.minLength(8)]]
	});

	public onSubmit() {
		console.log(this.myForm.value);
	};
}
