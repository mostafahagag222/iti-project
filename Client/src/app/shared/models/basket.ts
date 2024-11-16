
export interface IBasket {
    id: number;
    basketItems: IBasketItem[];
    userId: string;
    clientSecret?: string;
    paymentIntentId?: string;
    deliveryMethodId?: number;
    shippingPrice?: number;
}

export interface IBasketItem {
    id: number;
    price: number;
    quantity: number;
    productId: number;
    basketId: number;
}

export interface IBasketTotals {
    shipping: number,
    subtotal: number,
    total: number
}







