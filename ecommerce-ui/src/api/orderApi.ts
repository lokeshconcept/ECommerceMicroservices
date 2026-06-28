import axios from "axios";
import type { PlaceOrderRequest, PlaceOrderResponse } from "../types/order";

const orderApi = axios.create({
  baseURL: import.meta.env.VITE_ORDER_API_BASE_URL,
});

export const placeOrder = async (
  data: PlaceOrderRequest
): Promise<PlaceOrderResponse> => {
  const response = await orderApi.post<PlaceOrderResponse>("/api/orders", data);
  return response.data;
};