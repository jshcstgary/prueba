import { inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { ApiResponse, Person, PersonCreate } from "@types";

import { environment } from "@dev-environments";

@Injectable({
	providedIn: "root"
})
export class PersonService {
	private http = inject(HttpClient);

	private personUrl = environment.personUrl;

	public create(newPerson: PersonCreate): Observable<ApiResponse<Person>> {
		return this.http.post<ApiResponse<Person>>(this.personUrl, newPerson);
	}

	public getAll(): Observable<ApiResponse<Person[]>> {
		return this.http.get<ApiResponse<Person[]>>(this.personUrl);
	}

	public getById(idPerson: number): Observable<ApiResponse<Person>> {
		return this.http.get<ApiResponse<Person>>(`${this.personUrl}/${idPerson}`);
	}

	public delete(idPerson: number): Observable<ApiResponse<null>> {
		return this.http.delete<ApiResponse<null>>(`${this.personUrl}/${idPerson}`);
		// return this.http.get<ApiResponse<Person>>(`${this.personUrl}/1`);
	}
}
