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

export type UserCreate = {
	username: string;
	password: string;
};

export type User = Omit<UserCreate, "password"> & {
	id: number;
	mail: string;
	sessionActive: boolean;
	status: string;
};

export type PersonCount = {
	label: string;
	amount: number;
};
