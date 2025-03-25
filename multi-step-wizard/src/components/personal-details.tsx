"use client";
import React from "react";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { useRouter } from "next/navigation";
import { zodResolver } from "@hookform/resolvers/zod";
import { useFormContext } from "@/context/FormContext";

import { PersonalSchema } from "@/schemas/personal-schema";

import FormLayout from "./form-layout";
import Label from "./form-label";
import Input from "./form-input";
import Error from "./input-error";
import Button from "./button";

const MainForm = () => {
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<z.infer<typeof PersonalSchema>>({
        resolver: zodResolver(PersonalSchema),
    });
    const router = useRouter();
    const { first_name, last_name, phone } = errors;
    const { updateFormValues } = useFormContext();
    const onSubmit = (values: z.infer<typeof PersonalSchema>) => {
        console.log(values);
        updateFormValues(values);
        router.push("/address");    
    };

    return (
        <FormLayout>
            <form className="space-y-3" onSubmit={handleSubmit(onSubmit)}>
                <Label className="text-base">Personal information</Label>
                <div>
                    <Label htmlFor="first_name">First Name</Label>
                    <Input id="first_name" {...register("first_name")}/>
                    {first_name && <Error error={first_name.message}/>}
                </div>
                <div>
                    <Label htmlFor="last_name">Last Name</Label>
                    <Input id="last_name" {...register("last_name")}/>
                    {last_name && <Error error={last_name.message}/>}
                </div>
                <div>
                    <Label htmlFor="phone">Phone Number</Label>
                    <Input id="phone" {...register("phone")}/>
                    {phone && <Error error={phone.message}/>}
                </div>
                <Button type="submit">Next</Button>
            </form>
        </FormLayout>
    );
};

export default MainForm;
