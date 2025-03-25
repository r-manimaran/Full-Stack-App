'use client'; // This directive is required for client-side components in Next.js

import { Dispatch, SetStateAction } from "react";
import { createContext, useContext, useState } from "react";

// Define the shape of our form context data
interface FormValueType {
    // object to store all form values
    formValues: {};
    // function to update form values
    updateFormValues: (x: any) => void;
    // Track which step of the form we are on
    currentStep: number;
    // fuction to update the current step
    setCurrentStep: Dispatch<SetStateAction<number>>;
}
// Props interface for the FormProvider component
interface Props {
    // children represents any React componets that will be wrapped by this provider.
    children: React.ReactNode;
}

const FormContext = createContext<FormValueType | null>(null);

export const FormProvider = ({ children }: Props)=> {
    // Initialize state for form values with an empty object
    const[formValues, setFormValues] = useState({});
    // Initialize current step to 1 (first step)
    const[currentStep, setCurrentStep] = useState(1);

    // Function to update form values
    // It spreads the previous data and merges it with new data
    const updateFormValues = (updateData: any) => {
        setFormValues((prevData) => ({ ...prevData, ...updateData}));
    };

    // create an object with all our context values
    const values = {
        formValues,
        updateFormValues,
        currentStep,
        setCurrentStep,
    };
    // Return the context Provider with our values and children
    return <FormContext.Provider value={values}>{children}</FormContext.Provider>;
    
};

// custom hook to use our form context
export const useFormContext =() => {
    // get the context
    const context = useContext(FormContext);
    // If the context is null, it means we are trying to use the context outside of the formprovider
    if( context === null)
    {
        throw new Error("useFormContext must be used within a FormProvider");
    }
    // Return the context if it exists
    return context;
}