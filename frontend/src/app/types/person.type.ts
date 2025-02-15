import { User, UserCreate, UserUpdate } from "@types";

export type CreateResponse = {
	id: string;
};

export type PersonCreate = {
	identification: string;
	names: string;
	surnames: string;
	birthDate: Date;
	user: UserCreate;
};

export type Person = Omit<PersonCreate, "user"> & {
	id: number;
	user: User;
};

export type PersonUpdate = Omit<PersonCreate, "user"> & {
	id: number;
	user: UserUpdate;
};

export type PersonCount = {
	label: string;
	amount: number;
};
