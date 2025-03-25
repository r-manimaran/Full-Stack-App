"use client";
import React from "react";
import { useFormContext } from "@/context/FormContext";

import FormLayout from "./form-layout";
import Button from "./button";
import Label from "./form-label";

const Review = () => {
  const { formValues, currentStep } = useFormContext();
  return (
    <FormLayout>
      <Label>Form details</Label>
      <div className="w-full">
        <pre style={{ maxWidth: "100%", overflowX: "auto" }}>
          {JSON.stringify(formValues, null, 2)}
        </pre>
      </div>
      <Button className="text-center">Submit</Button>
    </FormLayout>
  );
};

export default Review;