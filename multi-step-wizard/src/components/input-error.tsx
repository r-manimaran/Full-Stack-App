import React from "react";

interface ErrorProps {
  error?: string;
}

const Error = ({ error }: ErrorProps) => {
  if (!error) return null;

  return <p className="mt-1 text-sm text-red-600">{error}</p>;
};

export default Error;
