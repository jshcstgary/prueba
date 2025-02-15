import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { catchError, map, of } from "rxjs";

import { RoleService } from "@services";

import { LocalStorageKeys } from "@constants";

import { Role, RoleOption } from "@types";

import { openToast } from "@lib";

export const notAuthenticatedGuard: CanActivateFn = (route, state) => {
	const roleService = inject(RoleService);
	const router = inject(Router);

	const idPerson = localStorage.getItem(LocalStorageKeys.idPerson);
	const idRole = localStorage.getItem(LocalStorageKeys.idRole);

	if ((idPerson === undefined || idPerson === null || idPerson === "") && (idRole === undefined || idRole === null || idRole === "")) {
		router.navigate(["/auth"]);

		return of(false);
	}

	return roleService.getById(Number(idRole!)).pipe(
		map(({ data }) => {
			const role: Role = data as Role;

			const requestedRoute = state.url;

			const routes = [...role.roleOptions.map((roleOption: RoleOption) => roleOption.link), "/users/user"];

			if (routes.some(roleOption => roleOption === requestedRoute)) {
				return true;
			} else {
				router.navigate(["/not-authorized"]);

				openToast("No tiene acceso a esta ruta", "warning");

				return false;
			}
		}),
		catchError(() => {
			router.navigate(["/not-authorized"]);

			openToast("No tiene acceso a esta ruta", "warning");

			return of(false);
		})
	);
};
