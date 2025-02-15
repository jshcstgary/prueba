import { Injectable } from "@angular/core";

import { saveAs } from "file-saver";
import * as XLSX from "xlsx";

@Injectable({
	providedIn: "root"
})
export class ExcelService {
	public exportExcelTemplate(fileName: string, headers: string[][]): void {
		const ws: XLSX.WorkSheet = XLSX.utils.aoa_to_sheet(headers);

		const wb: XLSX.WorkBook = XLSX.utils.book_new();
		XLSX.utils.book_append_sheet(wb, ws, "Plantilla");

		const excelBuffer = XLSX.write(wb, {
			bookType: "xlsx",
			type: "array"
		});

		const dataBlob: Blob = new Blob([excelBuffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });

		saveAs(dataBlob, `${fileName}.xlsx`);
	}
}
