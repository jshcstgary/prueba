export type ApiResponse<T> = {
	statusCode: number;
	statusMessage: string;
	errorMessage: string;
	data?: T;
};
