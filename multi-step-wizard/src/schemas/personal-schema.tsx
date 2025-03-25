import { z } from "zod";

export const PersonalSchema = z.object({
    first_name: z.string().min(1, {message:"First name is required"}),
    last_name: z.string().min(1, {message:"Last name is required"}),
    phone:z.string().min(1, {message:"Phone number is required"}),
});