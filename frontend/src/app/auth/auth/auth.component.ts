import { Component, inject, signal } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";

import { AuthService, RoleService } from "@services";

import { AuthData, Person, Role } from "@types";

import { LocalStorageKeys, Status } from "@constants";

import { openToast } from "@lib";

@Component({
	selector: "app-auth",
	standalone: false,
	templateUrl: "./auth.component.html"
})
export class AuthComponent {
	private router = inject(Router);

	private authService = inject(AuthService);

	private roleService = inject(RoleService);

	private formBuilder = inject(FormBuilder);

	public isLoading = signal(false);

	public showErrors = signal(false);

	public roles = signal<Role[]>([]);

	public myForm = this.formBuilder.group({
		username: ["", [Validators.required]],
		password: ["", [Validators.required]],
		idRole: ["", [Validators.required]]
	});

	constructor() {
		this.getRoles();
	}

	private getRoles(): void {
		this.isLoading.set(!this.isLoading());

		this.roleService.getAll(Status.Active).subscribe({
			next: (response) => {
				this.roles.set(response.data!);

				this.isLoading.set(!this.isLoading());
			},
			error: () => {
				openToast("No se pudo obtener los roles.", "error");

				this.isLoading.set(!this.isLoading());
			}
		});
	}

	public getUsernameError(): string {
		if (this.myForm.controls.username.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public getPasswordError(): string {
		if (this.myForm.controls.password.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public getRoleError(): string {
		if (this.myForm.controls.idRole.hasError("required")) {
			return "Campo requerido";
		}

		return "";
	}

	public onSubmit() {
		this.showErrors.set(false);

		if (this.myForm.invalid) {
			this.showErrors.set(this.myForm.invalid);

			openToast("Llene el formulario de forma correcta", "error");

			return;
		}

		this.isLoading.set(!this.isLoading());

		const authData: AuthData = {
			username: this.myForm.controls.username.value!,
			password: this.myForm.controls.password.value!,
			idRole: Number(this.myForm.controls.idRole.value!)
		};

		this.authService.signIn(authData).subscribe({
			next: (response) => {
				const person: Person = response.data! as Person;

				localStorage.setItem(LocalStorageKeys.idPerson, String(response.data!.id));
				localStorage.setItem(LocalStorageKeys.idRole, String(authData.idRole));
				localStorage.setItem(LocalStorageKeys.isAuthenticated, String(true));

				const roleOptions = person.user.roles.find((role: Role) => role.id === authData.idRole)!.roleOptions;

				this.authService.authenticatedPerson.set(person);
				this.authService.authenticatedIdRole.set(authData.idRole);
				this.authService.authenticatedRoleOptions.set(roleOptions);

				this.isLoading.set(!this.isLoading());

				this.router.navigate(["/"]);
			},
			error: (err) => {
				localStorage.removeItem("idPerson");
				localStorage.removeItem("idRole");
				localStorage.removeItem("isAuthenticated");

				openToast(err.error.errorMessage, "error");

				this.isLoading.set(!this.isLoading());
			}
		});
	}
}
