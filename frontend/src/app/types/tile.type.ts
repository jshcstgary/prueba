import { PersonCount } from "@types";

export type ColorTile = {
	colorFrom: string;
	colorTo: string;
};

export type Count = PersonCount & ColorTile;
