import { Injectable, signal } from "@angular/core";

@Injectable({
	providedIn: "root"
})
export class ModalService<T> {
	private _modalData = signal<T | null>(null);

	get modalData(): T | null {
		return this._modalData();
	}

	set modalData(newValue: T | null) {
		this._modalData.set(newValue);
	}
}
