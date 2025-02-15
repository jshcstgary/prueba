import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";

import { LocalStorageKeys } from "@constants";

export const authenticatedGuard: CanActivateFn = () => {
	const router = inject(Router);

	const idPerson = localStorage.getItem(LocalStorageKeys.idPerson);
	const idRole = localStorage.getItem(LocalStorageKeys.idRole);

	if ((idPerson === undefined || idPerson === null || idPerson === "") && (idRole === undefined || idRole === null || idRole === "")) {
		return true;
	} else {
		router.navigate(["/"]);

		return false;
	}
};
