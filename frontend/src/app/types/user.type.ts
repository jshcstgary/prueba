import { Role } from "@types";

export type UserCreate = {
	username: string;
	password: string;
	roles: Role[];
};

export type User = Omit<UserCreate, "password"> & {
	id: number;
	mail: string;
	sessionActive: boolean;
	status: string;
};

export type UserUpdate = UserCreate & {
	id: number;
	mail: string;
	sessionActive: boolean;
	status: string;
};
