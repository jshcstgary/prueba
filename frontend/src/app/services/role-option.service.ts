import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { Params, Status } from "@constants";

import { ApiResponse, RoleOption, RoleOptionCreate } from "@types";

import { environment } from "@dev-environments";

@Injectable({
	providedIn: "root"
})
export class RoleOptionService {
	private http = inject(HttpClient);

	private roleOptionUrl = environment.roleOptionUrl;

	public create(newRoleOption: RoleOptionCreate): Observable<ApiResponse<RoleOption>> {
		return this.http.post<ApiResponse<RoleOption>>(this.roleOptionUrl, newRoleOption);
	}

	public getAll(status?: Status): Observable<ApiResponse<RoleOption[]>> {
		let params = new HttpParams();

		if (status !== undefined) {
			params = params.append(Params.status, status);
		}

		return this.http.get<ApiResponse<RoleOption[]>>(this.roleOptionUrl, {
			params
		});
	}

	public getById(idRoleOption: number): Observable<ApiResponse<RoleOption>> {
		return this.http.get<ApiResponse<RoleOption>>(`${this.roleOptionUrl}/${idRoleOption}`);
	}

	public delete(idRoleOption: number): Observable<ApiResponse<null>> {
		return this.http.delete<ApiResponse<null>>(`${this.roleOptionUrl}/${idRoleOption}`);
	}
}
