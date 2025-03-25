"use client"
import { useFormContext } from "@/context/FormContext";
import PersonalDetails from "@/components/personal-details";
import AddressDetails from "@/components/address-details";
import PaymentDetails from "@/components/payment-details";
import Review from "@/components/review";
import  Stepper  from "@/components/stepper";

export default function Home() {
  const { currentStep } = useFormContext();

  const steps = [
    { label: "Personal", component: <PersonalDetails key="personal" /> },
    { label: "Address", component: <AddressDetails key="address" /> },
    { label: "Payment", component: <PaymentDetails key="payment" /> },
    { label: "Review", component: <Review key="review" /> },
  ];
  const stepLabels = steps.map(step => step.label);
  return (
    <main className="min-h-screen flex items-center justify-center p-4">
    <div className="w-full max-w-md">
    <Stepper />{steps[currentStep].component}</div>
  </main>
  );
}
