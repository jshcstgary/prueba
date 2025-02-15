import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { Params, Status } from "@constants";

import { ApiResponse, Role, RoleCreate } from "@types";

import { environment } from "@dev-environments";

@Injectable({
	providedIn: "root"
})
export class RoleService {
	private roleUrl = environment.roleUrl;

	private http = inject(HttpClient);

	public create(newRole: RoleCreate): Observable<ApiResponse<Role>> {
		return this.http.post<ApiResponse<Role>>(this.roleUrl, newRole);
	}

	public getAll(status?: Status): Observable<ApiResponse<Role[]>> {
		let params = new HttpParams();

		if (status !== undefined) {
			params = params.append(Params.Status, status);
		}

		return this.http.get<ApiResponse<Role[]>>(this.roleUrl, {
			params
		});
	}

	public getById(idRole: number): Observable<ApiResponse<Role>> {
		return this.http.get<ApiResponse<Role>>(`${this.roleUrl}/${idRole}`);
	}

	public update(roleUpdated: Role): Observable<ApiResponse<Role>> {
		return this.http.put<ApiResponse<Role>>(this.roleUrl, roleUpdated);
	}

	public delete(idRole: number): Observable<ApiResponse<null>> {
		return this.http.delete<ApiResponse<null>>(`${this.roleUrl}/${idRole}`);
	}
}
