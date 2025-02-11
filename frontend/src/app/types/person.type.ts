export type Person = {
	id: number;
	identification: string;
	names: string;
	surnames: string;
	birthDate: Date;
	user: User;
};

export type User = {
	id: number;
	username: string;
	mail: string;
	sessionActive: boolean;
	status: string;
};
