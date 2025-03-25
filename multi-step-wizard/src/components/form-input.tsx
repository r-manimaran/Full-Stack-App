import React from "react";

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  error?: string;
}

const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className = "", error, ...props }, ref) => {
    return (
      <input
        ref={ref}
        className={`w-full px-3 py-2 border rounded-md shadow-sm focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 ${
          error
            ? "border-red-500 focus:ring-red-500 focus:border-red-500"
            : "border-gray-300"
        } ${className}`}
        {...props}
      />
    );
  }
);

Input.displayName = "Input";

export default Input;
