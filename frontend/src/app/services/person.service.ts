import { inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { environment } from "../../environments/environment.development";

import { ApiResponse, Person } from "@types";

@Injectable({
	providedIn: "root"
})
export class PersonService {
	private http = inject(HttpClient);

	private apiUrl = environment.apiUrl;

	public getAll(): Observable<ApiResponse<Person[]>> {
		return this.http.get<ApiResponse<Person[]>>(`${this.apiUrl}/api/person`);
	}
}
