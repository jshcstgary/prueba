import { RoleOption } from "@types";

export type RoleCreate = {
	name: string;
	roleOptions: RoleOption[];
}

export type Role = RoleCreate & {
	id: number;
	status: string;
};
