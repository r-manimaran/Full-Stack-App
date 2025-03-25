"use client";
import AddressDetails from "@/components/address-details";

export default function AddressPage() {
  return (
    <main className="min-h-screen flex items-center justify-center p-4">
      <div className="w-full max-w-md">
        <AddressDetails />
      </div>
    </main>
  );
}
