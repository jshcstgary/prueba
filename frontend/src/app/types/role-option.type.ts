export type RoleOptionCreate = {
	name: string;
};

export type RoleOption = RoleOptionCreate & {
	id: number;
	status: string;
};
