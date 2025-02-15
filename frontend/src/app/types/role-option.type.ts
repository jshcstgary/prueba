export type RoleOptionCreate = {
	name: string;
	link: string;
};

export type RoleOption = RoleOptionCreate & {
	id: number;
	status: string;
};
