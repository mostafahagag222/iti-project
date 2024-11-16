export class productViewModel {
    constructor(id:number, name:string, price:number, quantity:number,
        pictureUrl:string, brand:string, type:string)
    {
        this.id = id;
        this.productName = name;
        this.price = price;
        this.quantity = quantity;
        this.pictureUrl = pictureUrl;
        this.brand = brand;
        this.type = type;
    }
    id:number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
}
