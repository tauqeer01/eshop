import { nanoid } from 'nanoid';
export type CartType={
    id:number
    items:CartItem[]
}
export type CartItem={
    [x: string]: any;
    productId:number
    productName:string
    price:number
    quantity:number
    pictureUrl:string
    brand:string
    type:string
}

export class Cart implements Cart{
    id=nanoid();
    items:CartItem[]=[];
}