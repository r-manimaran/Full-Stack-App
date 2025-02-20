"use client";

import { useSearchParams } from "next/navigation";
import { ProductDetails } from  "./ProductDetails";

export default function ProductPage() {
  const searchParams = useSearchParams();
  const productId = searchParams.get("id");

  if(!productId) {
    return <div>No product ID provided.</div>
  }

  return (
    <div className="min-h-screen bg-gray-900 p-8">
      <ProductDetails productId={productId} />
    </div>
  );
}
