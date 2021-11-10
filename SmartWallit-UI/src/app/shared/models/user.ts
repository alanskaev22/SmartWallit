import { Address } from "./address";

export interface IUser {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: string;
    address: Address;
}