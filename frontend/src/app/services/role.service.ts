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
	private http = inject(HttpClient);

	private roleUrl = environment.roleUrl;

	public create(newRole: RoleCreate): Observable<ApiResponse<Role>> {
		return this.http.post<ApiResponse<Role>>(this.roleUrl, newRole);
	}

	public getAll(): Observable<ApiResponse<Role[]>> {
		return this.http.get<ApiResponse<Role[]>>(this.roleUrl);
	}

	public getById(idRole: number): Observable<ApiResponse<Role>> {
		return this.http.get<ApiResponse<Role>>(`${this.roleUrl}/${idRole}`);
	}

	public delete(idRole: number): Observable<ApiResponse<null>> {
		return this.http.delete<ApiResponse<null>>(`${this.roleUrl}/${idRole}`);
	}
}
