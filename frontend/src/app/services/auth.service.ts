import { HttpClient } from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { Observable } from "rxjs";

import { ApiResponse, AuthData, Person, RoleOption } from "@types";

import { environment } from "@dev-environments";

@Injectable({
	providedIn: "root"
})
export class AuthService {
	private authUrl = environment.authUrl;

	private http = inject(HttpClient);

	public authenticatedPerson = signal<Person | null>(null);

	public authenticatedIdRole = signal(0);

	public authenticatedRoleOptions = signal<RoleOption[]>([]);

	public signIn(authData: AuthData): Observable<ApiResponse<Person>> {
		return this.http.post<ApiResponse<Person>>(`${this.authUrl}/sign_in`, authData);
	}

	public signOut(): Observable<ApiResponse<void>> {
		return this.http.get<ApiResponse<void>>(`${this.authUrl}/sign_out/${this.authenticatedPerson()!.id}`);
	}
}
