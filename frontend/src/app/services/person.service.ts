import { inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { ApiResponse, Person, PersonCount, PersonsCreateResponse, PersonCreate, PersonUpdate } from "@types";

import { environment } from "@dev-environments";

@Injectable({
	providedIn: "root"
})
export class PersonService {
	private personUrl = environment.personUrl;

	private http = inject(HttpClient);

	public create(newPersons: PersonCreate[]): Observable<ApiResponse<PersonsCreateResponse>> {
		return this.http.post<ApiResponse<PersonsCreateResponse>>(this.personUrl, newPersons);
	}

	public getAll(): Observable<ApiResponse<Person[]>> {
		return this.http.get<ApiResponse<Person[]>>(this.personUrl);
	}

	public getCount(): Observable<ApiResponse<PersonCount[]>> {
		return this.http.get<ApiResponse<PersonCount[]>>(`${this.personUrl}/count`);
	}

	public getById(idPerson: number): Observable<ApiResponse<Person>> {
		return this.http.get<ApiResponse<Person>>(`${this.personUrl}/${idPerson}`);
	}

	public update(personUpdate: PersonUpdate): Observable<ApiResponse<Person>> {
		return this.http.put<ApiResponse<Person>>(this.personUrl, personUpdate);
	}

	public delete(idPerson: number): Observable<ApiResponse<null>> {
		return this.http.delete<ApiResponse<null>>(`${this.personUrl}/${idPerson}`);
	}
}
