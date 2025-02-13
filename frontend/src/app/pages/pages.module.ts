import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { DashboardComponent, HomeComponent, PagesRoutingModule, TileComponent } from "@pages";

@NgModule({
	declarations: [HomeComponent, DashboardComponent, TileComponent],
	imports: [CommonModule, PagesRoutingModule]
})
export class PagesModule {}
