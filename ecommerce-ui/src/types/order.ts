export interface PlaceOrderRequest {
    productId: number;
    quantity: number;
}

export interface Order {
    orderId: string;
    productId: number;
    quantity: number;
    createdAt: string;
}

export interface PlaceOrderResponse {
    message: string;
    order: Order;
}