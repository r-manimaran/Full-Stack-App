import axios from 'axios';
import { GasPurchase, Summary, PriceStats, StationStats, MonthlyStat, YearlyStat } from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const gasPurchasesApi = {
  getAll: async (): Promise<GasPurchase[]> => {
    const response = await api.get<GasPurchase[]>('/api/gaspurchases');
    return response.data;
  },

  getById: async (id: number): Promise<GasPurchase> => {
    const response = await api.get<GasPurchase>(`/api/gaspurchases/${id}`);
    return response.data;
  },

  create: async (purchase: Omit<GasPurchase, 'id'>): Promise<GasPurchase> => {
    const response = await api.post<GasPurchase>('/api/gaspurchases', purchase);
    return response.data;
  },

  update: async (id: number, purchase: Omit<GasPurchase, 'id'>): Promise<GasPurchase> => {
    const response = await api.put<GasPurchase>(`/api/gaspurchases/${id}`, { ...purchase, id });
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/api/gaspurchases/${id}`);
  },
};

export const analyticsApi = {
  getSummary: async (): Promise<Summary> => {
    const response = await api.get<Summary>('/api/analytics/summary');
    return response.data;
  },

  getPriceStats: async (): Promise<PriceStats> => {
    const response = await api.get<PriceStats>('/api/analytics/pricestats');
    return response.data;
  },

  getTrends: async (period: 'daily' | 'monthly' | 'yearly' = 'daily'): Promise<any> => {
    const response = await api.get(`/api/analytics/trends?period=${period}`);
    return response.data;
  },

  getByStation: async (): Promise<StationStats[]> => {
    const response = await api.get<StationStats[]>('/api/analytics/bystation');
    return response.data;
  },

  getMonthly: async (): Promise<MonthlyStat[]> => {
    const response = await api.get<MonthlyStat[]>('/api/analytics/monthly');
    return response.data;
  },

  getYearly: async (): Promise<YearlyStat[]> => {
    const response = await api.get<YearlyStat[]>('/api/analytics/yearly');
    return response.data;
  },
};
