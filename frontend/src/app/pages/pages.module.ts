import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { PagesRoutingModule } from "./pages-routing.module";

import { DashboardComponent, HomeComponent, TileComponent } from "@pages";

@NgModule({
	declarations: [HomeComponent, DashboardComponent, TileComponent],
	imports: [CommonModule, PagesRoutingModule]
})
export class PagesModule {}
