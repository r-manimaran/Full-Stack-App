// app/products/details/ProductDetails.tsx
'use client';

import { useEffect, useState } from 'react';

interface Product {
  id: string;
  name: string;
  description: string;
  details: {
    brand: string;
    category: string;
    subCategory: string;
    manufacturer: string;
    countryOfOrigin: string;
    tags: string[];
  };
  pricing: {
    basePrice: number;
    discountedPrice: number | null;
    currency: string;
    isOnSale: boolean;
    saleEndAt: string | null;
  };
  inventory: {
    stockQuanitity: number;
    sku: string;
    warehouseLocation: string;
    isInStock: boolean;
    reorderPoint: number;
    needsReorder: boolean;
  };
  specifications: {
    dimensions: {
      Width: string;
      Height: string;
      Length: string;
    };
    weightInKg: number;
    materials: string[];
    technicalSpecs: {
      Color: string;
      "Model Number": string;
      "Material Type": string;
    };
  };
  createdOn: string;
  updatedOn: string | null;
}

export function ProductDetails({ productId }: { productId: string }) {
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const response = await fetch(`https://productsapi-ahc6dsaua5bhbzc7.canadacentral-01.azurewebsites.net/api/v1/Products/${productId}`);
        if (!response.ok) {
          throw new Error('Product not found');
        }
        const data = await response.json();
        setProduct(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to fetch product');
      } finally {
        setLoading(false);
      }
    };

    fetchProduct();
  }, [productId]);

  if (loading) {
    return <div className="text-center text-gray-400">Loading product details...</div>;
  }

  if (error) {
    return <div className="text-red-500">Error: {error}</div>;
  }

  if (!product) {
    return <div className="text-gray-400">Product not found</div>;
  }

  return (
    <div className="max-w-4xl mx-auto text-gray-100">
      <Link href="/products" className="mb-4 inline-block text-blue-400 hover:text-blue-300">
        &larr; Back to Products
      </Link>

      <h1 className="text-4xl font-bold mb-6">{product.name}</h1>
      
      <div className="space-y-6">
        <section className="bg-gray-800 p-6 rounded-lg">
          <h2 className="text-2xl font-semibold mb-4">Basic Information</h2>
          <p className="text-gray-300">{product.description}</p>
          <div className="grid grid-cols-2 gap-4 mt-4">
            <div>
              <p className="text-gray-400">Brand: {product.details.brand}</p>
              <p className="text-gray-400">Category: {product.details.category}</p>
              <p className="text-gray-400">Manufacturer: {product.details.manufacturer}</p>
            </div>
            <div>
              <p className="text-gray-400">SKU: {product.inventory.sku}</p>
              <p className="text-gray-400">Stock: {product.inventory.stockQuanitity}</p>
              <p className={product.inventory.isInStock ? 'text-green-400' : 'text-red-400'}>
                {product.inventory.isInStock ? 'In Stock' : 'Out of Stock'}
              </p>
            </div>
          </div>
        </section>

        <section className="bg-gray-800 p-6 rounded-lg">
          <h2 className="text-2xl font-semibold mb-4">Pricing</h2>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <p className="text-gray-400">Base Price: ${product.pricing.basePrice}</p>
              {product.pricing.isOnSale && (
                <p className="text-green-400">
                  On Sale! {product.pricing.discountedPrice && 
                    `Discounted Price: $${product.pricing.discountedPrice}`}
                </p>
              )}
              <p className="text-gray-400">Currency: {product.pricing.currency}</p>
            </div>
            {product.pricing.saleEndAt && (
              <div>
                <p className="text-gray-400">Sale Ends: {new Date(product.pricing.saleEndAt).toLocaleDateString()}</p>
              </div>
            )}
          </div>
        </section>

        <section className="bg-gray-800 p-6 rounded-lg">
          <h2 className="text-2xl font-semibold mb-4">Specifications</h2>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <h3 className="font-semibold mb-2">Dimensions</h3>
              <p className="text-gray-400">Width: {product.specifications.dimensions.Width}</p>
              <p className="text-gray-400">Height: {product.specifications.dimensions.Height}</p>
              <p className="text-gray-400">Length: {product.specifications.dimensions.Length}</p>
            </div>
            <div>
              <h3 className="font-semibold mb-2">Technical Specs</h3>
              <p className="text-gray-400">Weight: {product.specifications.weightInKg} kg</p>
              <p className="text-gray-400">Color: {product.specifications.technicalSpecs.Color}</p>
              <p className="text-gray-400">Model: {product.specifications.technicalSpecs["Model Number"]}</p>
            </div>
          </div>
        </section>

        <section className="bg-gray-800 p-6 rounded-lg">
          <h2 className="text-2xl font-semibold mb-4">Additional Information</h2>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <p className="text-gray-400">Created: {new Date(product.createdOn).toLocaleDateString()}</p>
              {product.updatedOn && (
                <p className="text-gray-400">Updated: {new Date(product.updatedOn).toLocaleDateString()}</p>
              )}
            </div>
            <div>
              <p className="text-gray-400">Country of Origin: {product.details.countryOfOrigin}</p>
              <p className="text-gray-400">Warehouse: {product.inventory.warehouseLocation}</p>
            </div>
          </div>
        </section>
      </div>
    </div>
  );
}