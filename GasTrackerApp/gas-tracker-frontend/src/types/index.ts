export interface GasPurchase {
  id: number;
  pricePerGallon: number;
  gallonsPurchased: number;
  dateAndTime: string;
  totalPrice: number;
  fuelStation: string;
}

export interface Summary {
  totalSpent: number;
  totalGallons: number;
  averagePricePerGallon: number;
  totalPurchases: number;
  averageGallonsPerPurchase: number;
}

export interface PriceStats {
  minPrice: number | null;
  maxPrice: number | null;
  minPricePurchase: GasPurchase | null;
  maxPricePurchase: GasPurchase | null;
}

export interface StationStats {
  station: string;
  totalSpent: number;
  totalGallons: number;
  averagePricePerGallon: number;
  purchaseCount: number;
  lastPurchaseDate: string;
}

export interface MonthlyStat {
  year: number;
  month: number;
  label: string;
  totalSpent: number;
  totalGallons: number;
  averagePricePerGallon: number;
  purchaseCount: number;
}

export interface YearlyStat {
  year: number;
  totalSpent: number;
  totalGallons: number;
  averagePricePerGallon: number;
  purchaseCount: number;
}
