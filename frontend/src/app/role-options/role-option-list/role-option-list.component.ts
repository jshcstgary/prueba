import { Component, inject, OnInit, signal, viewChild } from "@angular/core";

import { ConfirmationModalComponent } from "@components";

import { RoleOptionService } from "@services";

import { RoleOption } from "@types";

import { openToast } from "@lib";

import { Status } from "@constants";
import { RoleOptionFormComponent } from "@role-options";

@Component({
	selector: "app-role-option-list",
	standalone: false,
	templateUrl: "./role-option-list.component.html"
})
export class RoleOptionListComponent implements OnInit {
	private roleOptionService = inject(RoleOptionService);

	public status = Status;

	public roleOptions = signal<RoleOption[]>([]);

	public isLoading = signal<boolean>(false);

	public idRole = signal<number>(0);

	public roleOptionFormModal = viewChild<RoleOptionFormComponent>("roleOptionFormModal");

	public deleteRoleOptionModal = viewChild<ConfirmationModalComponent>("deleteRoleOptionModal");

	ngOnInit() {
		this.getRoleOptions();
	}

	private getRoleOptions(): void {
		this.isLoading.set(!this.isLoading());

		this.roleOptionService.getAll().subscribe({
			next: (response) => {
				this.roleOptions.set(response.data!);

				this.isLoading.set(!this.isLoading());
			},
			error: () => {
				this.roleOptions.set([]);

				this.isLoading.set(!this.isLoading());

				openToast("No se pudo obtener los registros.", "error");
			}
		});
	}

	private deleteRoleOption(): void {
		this.deleteRoleOptionModal()?.isLoading.set(!this.deleteRoleOptionModal()?.isLoading());

		this.roleOptionService.delete(this.idRole()).subscribe({
			next: () => {
				this.deleteRoleOptionModal()?.isLoading.set(!this.deleteRoleOptionModal()?.isLoading());
				this.deleteRoleOptionModal()?.close();

				openToast("Registro eliminado.", "success");

				this.getRoleOptions();
			},
			error: (err) => {
				this.deleteRoleOptionModal()?.isLoading.set(!this.deleteRoleOptionModal()?.isLoading());
				this.deleteRoleOptionModal()?.close();

				openToast(err.error.errorMessage, "error");

				this.getRoleOptions();
			}
		});
	}

	public openRoleOptionFormModal(idRoleOption: number | null): void {
		this.roleOptionFormModal()?.idRoleOption.set(idRoleOption);

		this.roleOptionFormModal()?.openModal();
	}

	public openDeleteRoleOptionModal(idRoleOption: number): void {
		this.idRole.set(idRoleOption);

		this.deleteRoleOptionModal()?.type.set("error");

		this.deleteRoleOptionModal()?.open();
	}

	public onCloseRoleOptionForm(reload: boolean): void {
		if (reload) {
			this.getRoleOptions();
		}
	}

	public onConfirmation(isConfirmed: boolean): void {
		if (!isConfirmed) {
			this.deleteRoleOptionModal()?.close();

			return;
		}

		this.deleteRoleOption();
	}
}
