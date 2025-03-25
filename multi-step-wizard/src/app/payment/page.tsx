"use client";
import PaymentDetails from "@/components/payment-details";


export default function PaymentPage() {
  return (
    <main className="min-h-screen flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        <PaymentDetails />
      </div>
    </main>
  );
}