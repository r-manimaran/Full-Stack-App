import { SearchParams } from "next/dist/server/request/search-params";

import Image from "next/image";
import Link from "next/link";

interface searchParams {
  page?:string;
}

async function getProducts(page=1) {
  const res = await fetch(`https://productsapi-ahc6dsaua5bhbzc7.canadacentral-01.azurewebsites.net/api/v1/Products?pageNumber=${page}&pageSize=3`,
    {
      cache: "no-store",
      next: { revalidate: 0},
      headers: {
        "Content-Type": "application/json",
      }
    }
  );

  if(!res.ok){
    throw new Error("Failed to fetch products");
  }
  return res.json();
}

export default async function ProductsPage({searchParams}:{searchParams:SearchParams}) {
 const currentPage = Number(searchParams.page)|| 1;
 const data = await getProducts(currentPage);

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">Products</h1>
    
      <div className="grid gap-4 mb-6">
        {data.items.map((product) => (
          <div key={product.id} className="border p-4 rounded-lg shadow-sm">
            <h2 className="text-xl font-semibold">{product.name}</h2>
            <p className="text-gray-600 mt-2">{product.description}</p>
            <div className="mt-2">
              <span className="font-medium">
                Price: ${product.pricing.basePrice}
              </span>
              <span className={`ml-4 ${product.inventory.isInStock ? 'text-green-600' : 'text-red-600'}`}>
                {product.inventory.isInStock ? 'In Stock' : 'Out of Stock'}
              </span>
            </div>
          </div>
        ))}
      </div>

      <div className="flex justify-center gap-2">
        <Link
          href={`/products?page=${currentPage - 1}`}
          className={`px-4 py-2 rounded ${currentPage === 1 ? 'bg-gray-300 cursor-not-allowed' : 'bg-blue-500 text-white hover:bg-blue-600'}`}
          aria-disabled={currentPage === 1}
        >
          Previous
        </Link>
        
        {Array.from({ length: data.totalPages }, (_, i) => (
          <Link
            key={i + 1}
            href={`/products?page=${i + 1}`}
            className={`px-4 py-2 rounded ${
              currentPage === i + 1 
                ? 'bg-blue-500 text-white' 
                : 'bg-gray-200 hover:bg-gray-300'
            }`}
          >
            {i + 1}
          </Link>
        ))}
        
        <Link
          href={`/products?page=${currentPage + 1}`}
          className={`px-4 py-2 rounded ${currentPage === data.totalPages ? 'bg-gray-300 cursor-not-allowed' : 'bg-blue-500 text-white hover:bg-blue-600'}`}
          aria-disabled={currentPage === data.totalPages}
        >
          Next
        </Link>
      </div>
    </div>
  );
}
