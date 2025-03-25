"use client";
import React from "react";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useRouter } from "next/navigation";

import { paymentSchema } from "@/schemas/payment-schema";
import { useFormContext } from "@/context/FormContext";

import FormLayout from "./form-layout";
import Label from "./form-label";
import Error from "./input-error";
import Input from "./form-input";

import Button from "./button";
const PaymentDetails  = () => {
  const { setCurrentStep, updateFormValues } = useFormContext();
  const router = useRouter();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<z.infer<typeof paymentSchema>>({
    resolver: zodResolver(paymentSchema),
  });
  const { card_number, card_holder, expiration_date, cvv } = errors;
  const onSubmit = (values: z.infer<typeof paymentSchema>) => {
    updateFormValues(values);
    setCurrentStep((prev) => prev + 1);
    router.push("/review");
  };

  const handlePrevious = () => {
    router.push("/address");
    setCurrentStep((prev) => prev - 1);
  };

  return (
    <FormLayout>
      <form className="space-y-3" onSubmit={handleSubmit(onSubmit)}>
        <div>
          <Label htmlFor="card_number">Credit card number</Label>
          <Input {...register("card_number")} id="card_number" />
          {card_number && <Error error={card_number.message} />}
        </div>
        <div>
          <Label htmlFor="card_holder">Credit card Holder</Label>
          <input {...register("card_holder")} id="card_holder" />
          {card_holder && <Error error={card_holder.message} />}
        </div>
        <div className="flex gap-2">
          <div>
            <Label htmlFor="expiration_date">Expiry date</Label>
            <input {...register("expiration_date")} id="expiration_date" />
            {expiration_date && <Error error={expiration_date.message} />}
          </div>
          <div>
            <Label htmlFor="cvv">Cvv</Label>
            <input {...register("cvv")} id="cvv" />
            {cvv && <Error error={cvv.message} />}
          </div>
        </div>

        <div className="flex justify-between h-auto items-center">
          <Button type="button" onClick={handlePrevious}>
            previous
          </Button>
          <Button type="submit">Next</Button>
        </div>
      </form>
    </FormLayout>
  );
};
export default PaymentDetails ;