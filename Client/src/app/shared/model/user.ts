export type User={
    fistName:string;
    lastName:string;
    email:string;
    address:Address
}
export type Address={
    line1:string;
    line2?:string;
    city:string;
    state:string;
    postalCode:string;
    country:string
}