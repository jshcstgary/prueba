import { Component, inject, OnInit, signal, viewChild } from "@angular/core";
import { forkJoin } from "rxjs";

import { RoleFormComponent } from "@roles";

import { ConfirmationModalComponent } from "@components";

import { RoleOptionService, RoleService } from "@services";

import { Role, RoleOption } from "@types";

import { Status } from "@constants";

import { openToast } from "@lib";

@Component({
	selector: "app-role-list",
	standalone: false,
	templateUrl: "./role-list.component.html"
})
export class RoleListComponent implements OnInit {
	public status = Status;

	public roles = signal<Role[]>([]);

	public roleOptions = signal<RoleOption[]>([]);

	public isLoading = signal<boolean>(false);

	public idRole = signal<number>(0);

	public roleFormModal = viewChild<RoleFormComponent>("roleFormModal");

	public deleteRoleModal = viewChild<ConfirmationModalComponent>("deleteRoleModal");

	private roleService = inject(RoleService);

	private roleOptionService = inject(RoleOptionService);

	ngOnInit() {
		this.getData();
	}

	private getData(): void {
		this.isLoading.set(!this.isLoading());

		forkJoin([this.roleService.getAll(), this.roleOptionService.getAll(this.status.Active)]).subscribe({
			next: ([roles, roleOptions]) => {
				this.roles.set(roles.data!);
				this.roleOptions.set(roleOptions.data!);

				this.isLoading.set(!this.isLoading());
			}
		});
	}

	public openRoleFormModal(idRole: number | null): void {
		this.roleFormModal()?.idRole.set(idRole);
		this.roleFormModal()?.roleOptions.set(this.roleOptions());

		this.roleFormModal()?.open();
	}

	public openDeleteRoleModal(idRole: number): void {
		this.idRole.set(idRole);

		this.deleteRoleModal()?.type.set("error");

		this.deleteRoleModal()?.open();
	}

	public onCloseRoleForm(reload: boolean): void {
		if (reload) {
			this.getData();
		}
	}

	public onConfirmation(isConfirmed: boolean): void {
		if (!isConfirmed) {
			this.deleteRoleModal()?.close();

			return;
		}

		this.deleteRole();
	}

	private deleteRole(): void {
		this.deleteRoleModal()?.isLoading.set(!this.deleteRoleModal()?.isLoading());

		this.roleService.delete(this.idRole()).subscribe({
			next: () => {
				this.deleteRoleModal()?.isLoading.set(!this.deleteRoleModal()?.isLoading());
				this.deleteRoleModal()?.close();

				openToast("Registro eliminado.", "success");

				this.getData();
			},
			error: (err) => {
				this.deleteRoleModal()?.isLoading.set(!this.deleteRoleModal()?.isLoading());
				this.deleteRoleModal()?.close();

				openToast(err.error.errorMessage, "error");

				this.getData();
			}
		});
	}
}
