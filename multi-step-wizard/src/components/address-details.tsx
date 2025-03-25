"use client";
import React from "react";
import { z } from "zod";

import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import { useRouter } from "next/navigation";

import { addressSchema } from "@/schemas/address-schema";
import { useFormContext } from "@/context/FormContext";
import Error from "./input-error";

import FormLayout from "./form-layout";
import Label from "./form-label";
import Button from "./button";
import Input from "./form-input";

const Contact = () => {
    const router = useRouter();

    const { setCurrentStep, updateFormValues } = useFormContext();
  
    const { register, handleSubmit, formState: { errors },} = useForm<z.infer<typeof addressSchema>>({
      resolver: zodResolver(addressSchema),
    });

    const { street, city, state, zip, country } = errors;

    const onSubmit = (values: z.infer<typeof addressSchema>) => {
      updateFormValues(values);
      setCurrentStep((prev) => prev + 1);
      router.push("/payment");
    };
    const handlePrevious = () => {
        router.push("/");
        setCurrentStep((prev) => prev - 1);
      };
      return (
        <FormLayout>
          <form className="space-y-3" onSubmit={handleSubmit(onSubmit)}>
            <div>
              <Label htmlFor="street">Street</Label>
              <Input {...register("street")} id="street" />
              {street && <Error error={street.message} />}
            </div>
            <div>
              <Label htmlFor="city">City</Label>
              <Input {...register("city")} id="city" />
              {city && <Error error={city.message} />}
            </div>           
            <div>
              <Label htmlFor="state">State</Label>
              <Input {...register("state")} id="state" />
              {state && <Error error={state.message} />}
            </div>
            <div>
              <Label htmlFor="zip">Zip</Label>
              <Input {...register("zip")} id="zip" />
              {zip && <Error error={zip.message} />}
            </div>
            <div>
              <Label htmlFor="country">Country</Label>
              <Input {...register("country")} id="country" />
              {country && <Error error={country.message} />}
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
    
    export default Contact;