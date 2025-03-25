import { z } from "zod";

export const paymentSchema = z.object({
  card_number: z.string().min(16, "Card number must be 16 digits"),
  card_holder: z.string().min(1, "Card holder name is required"),
  expiration_date: z.string().min(5, "Expiration date is required"),
  cvv: z.string().min(3, "CVV is required"), 
});