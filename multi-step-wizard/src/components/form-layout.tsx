import React from "react";

interface FormLayoutProps {
  children: React.ReactNode;
}

const FormLayout = ({ children }: FormLayoutProps) => {
  return (
    <div className="max-w-md w-full mx-auto p-6 bg-white rounded-lg shadow-md">
      {children}
    </div>
  );
};

export default FormLayout;
