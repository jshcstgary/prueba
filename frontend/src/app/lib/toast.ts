import { Notyf } from "notyf";

export function openToast(message: string, type: "info" | "success" | "warning" | "error" = "info", duration = 3500) {
	const notyf = new Notyf();

	notyf.open({
		type,
		message,
		duration,
		ripple: true,
		dismissible: true
	});
}
