import { useState } from "react";
import { placeOrder } from "../api/orderApi";
import type { PlaceOrderResponse } from "../types/order";

export default function PlaceOrder() {
  const [productId, setProductId] = useState<number>(1);
  const [quantity, setQuantity] = useState<number>(1);
  const [result, setResult] = useState<PlaceOrderResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handlePlaceOrder = async (e: React.FormEvent) => {
    e.preventDefault();
    console.log("API URL:", import.meta.env.VITE_ORDER_API_BASE_URL);
    try {
      setLoading(true);
      setError("");
      setResult(null);

      const data = await placeOrder({
        productId,
        quantity,
      });

      setResult(data);
    } catch {
      setError("Order place nahi hua. API URL/CORS check karo.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: "40px auto" }}>
      <h2>Place Order CD Test</h2>

      <form onSubmit={handlePlaceOrder}>
        <div>
          <label>Product Id</label>
          <input
            type="number"
            value={productId}
            onChange={(e) => setProductId(Number(e.target.value))}
          />
        </div>

        <div>
          <label>Quantity</label>
          <input
            type="number"
            value={quantity}
            onChange={(e) => setQuantity(Number(e.target.value))}
          />
        </div>

        <button type="submit" disabled={loading}>
          {loading ? "Placing..." : "Place Order"}
        </button>
      </form>

      {error && <p style={{ color: "red" }}>{error}</p>}

      {result && (
        <div>
          <h3>Order Placed</h3>
          <p>Order Id: {result.order.orderId}</p>
          <p>Product Id: {result.order.productId}</p>
          <p>Quantity: {result.order.quantity}</p>
          <p>Created At: {result.order.createdAt}</p>
        </div>
      )}
    </div>
  );
}