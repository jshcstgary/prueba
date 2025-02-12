import { CommonModule } from "@angular/common";
import { Component, ElementRef, EventEmitter, Output, signal, viewChild } from "@angular/core";

import { HSOverlay } from "flyonui/flyonui";

@Component({
	selector: "app-confirmation-modal",
	imports: [CommonModule],
	templateUrl: "./confirmation-modal.component.html"
})
export class ConfirmationModalComponent {
	public isLoading = signal(false);

	public type = signal<"default" | "info" | "success" | "warning" | "error">("default");

	public deletePersonModal = viewChild<ElementRef<HTMLElement>>("deletePerson");

	@Output() public readonly onConfirmation = new EventEmitter<boolean>();

	public open(): void {
		const deletePersonModal = new HSOverlay(this.deletePersonModal()!.nativeElement);

		deletePersonModal.open();
	}

	public close(): void {
		const deletePersonModal = new HSOverlay(this.deletePersonModal()!.nativeElement);

		deletePersonModal.close();
	}

	public confirm(isConfirmed: boolean): void {
		this.onConfirmation.emit(isConfirmed);
	}
}
