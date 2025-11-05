import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Paper,
  Typography,
  Box,
  Card,
  CardContent,
  Alert,
  Button,
  ToggleButton,
  ToggleButtonGroup,
} from '@mui/material';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement,
} from 'chart.js';
import { Line, Bar, Doughnut } from 'react-chartjs-2';
import { Add as AddIcon } from '@mui/icons-material';
import { analyticsApi } from '../services/api';
import { Summary, PriceStats, StationStats, MonthlyStat, YearlyStat } from '../types';
import { format } from 'date-fns';
import StationLogo from './StationLogo';

// Register Chart.js components
ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement
);

const AnalyticsDashboard: React.FC = () => {
  const navigate = useNavigate();
  const [summary, setSummary] = useState<Summary | null>(null);
  const [priceStats, setPriceStats] = useState<PriceStats | null>(null);
  const [stationStats, setStationStats] = useState<StationStats[]>([]);
  const [monthlyStats, setMonthlyStats] = useState<MonthlyStat[]>([]);
  const [yearlyStats, setYearlyStats] = useState<YearlyStat[]>([]);
  const [trends, setTrends] = useState<any[]>([]);
  const [trendPeriod, setTrendPeriod] = useState<'daily' | 'monthly' | 'yearly'>('daily');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadAnalytics();
  }, [trendPeriod]);

  const loadAnalytics = async () => {
    try {
      setLoading(true);
      const [summaryData, priceStatsData, stationData, monthlyData, yearlyData, trendsData] =
        await Promise.all([
          analyticsApi.getSummary(),
          analyticsApi.getPriceStats(),
          analyticsApi.getByStation(),
          analyticsApi.getMonthly(),
          analyticsApi.getYearly(),
          analyticsApi.getTrends(trendPeriod),
        ]);

      setSummary(summaryData);
      setPriceStats(priceStatsData);
      setStationStats(stationData);
      setMonthlyStats(monthlyData);
      setYearlyStats(yearlyData);
      // Ensure trends is always an array
      setTrends(Array.isArray(trendsData) ? trendsData : []);
    } catch (err) {
      setError('Failed to load analytics');
    } finally {
      setLoading(false);
    }
  };

  const priceTrendsData = {
    labels: (Array.isArray(trends) ? trends : []).map((t: any) => {
      if (trendPeriod === 'daily' && t.date) {
        return format(new Date(t.date), 'MMM dd');
      }
      return t.label || '';
    }),
    datasets: [
      {
        label: 'Price per Gallon ($)',
        data: (Array.isArray(trends) ? trends : []).map((t: any) => (trendPeriod === 'daily' ? t.price : t.averagePrice)),
        borderColor: 'rgb(75, 192, 192)',
        backgroundColor: 'rgba(75, 192, 192, 0.2)',
        tension: 0.1,
      },
    ],
  };

  const spendingByStationData = {
    labels: stationStats.map((s) => s.station),
    datasets: [
      {
        label: 'Total Spent ($)',
        data: stationStats.map((s) => s.totalSpent),
        backgroundColor: [
          'rgba(255, 99, 132, 0.6)',
          'rgba(54, 162, 235, 0.6)',
          'rgba(255, 206, 86, 0.6)',
          'rgba(75, 192, 192, 0.6)',
          'rgba(153, 102, 255, 0.6)',
          'rgba(255, 159, 64, 0.6)',
        ],
      },
    ],
  };

  const monthlySpendingData = {
    labels: monthlyStats.map((m) => m.label),
    datasets: [
      {
        label: 'Total Spent ($)',
        data: monthlyStats.map((m) => m.totalSpent),
        backgroundColor: 'rgba(54, 162, 235, 0.6)',
      },
    ],
  };

  const yearlySpendingData = {
    labels: yearlyStats.map((y) => y.year.toString()),
    datasets: [
      {
        label: 'Total Spent ($)',
        data: yearlyStats.map((y) => y.totalSpent),
        backgroundColor: 'rgba(75, 192, 192, 0.6)',
      },
    ],
  };

  const chartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top' as const,
      },
      title: {
        display: true,
        text: '',
      },
    },
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Typography>Loading analytics...</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4">Analytics Dashboard</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => navigate('/purchases/new')}
        >
          Add Purchase
        </Button>
      </Box>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }} onClose={() => setError(null)}>
          {error}
        </Alert>
      )}

      {/* Summary Cards */}
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)', md: 'repeat(4, 1fr)' },
          gap: 3,
          mb: 3,
        }}
      >
        <Card>
          <CardContent>
            <Typography color="textSecondary" gutterBottom>
              Total Spent
            </Typography>
            <Typography variant="h4">
              ${summary?.totalSpent.toFixed(2) || '0.00'}
            </Typography>
          </CardContent>
        </Card>
        <Card>
          <CardContent>
            <Typography color="textSecondary" gutterBottom>
              Total Gallons
            </Typography>
            <Typography variant="h4">
              {summary?.totalGallons.toFixed(2) || '0.00'}
            </Typography>
          </CardContent>
        </Card>
        <Card>
          <CardContent>
            <Typography color="textSecondary" gutterBottom>
              Avg Price/Gallon
            </Typography>
            <Typography variant="h4">
              ${summary?.averagePricePerGallon.toFixed(2) || '0.00'}
            </Typography>
          </CardContent>
        </Card>
        <Card>
          <CardContent>
            <Typography color="textSecondary" gutterBottom>
              Total Purchases
            </Typography>
            <Typography variant="h4">{summary?.totalPurchases || 0}</Typography>
          </CardContent>
        </Card>
      </Box>

      {/* Price Statistics */}
      {priceStats && (priceStats.minPrice !== null || priceStats.maxPrice !== null) && (
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: { xs: '1fr', md: 'repeat(2, 1fr)' },
            gap: 3,
            mb: 3,
          }}
        >
          <Box>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Minimum Price
              </Typography>
              <Typography variant="h4" color="primary">
                ${priceStats.minPrice?.toFixed(2)}
              </Typography>
              {priceStats.minPricePurchase && (
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mt: 1 }}>
                  <StationLogo station={priceStats.minPricePurchase.fuelStation} size={20} />
                  <Typography variant="body2" color="textSecondary">
                    {priceStats.minPricePurchase.fuelStation} -{' '}
                    {format(new Date(priceStats.minPricePurchase.dateAndTime), 'MMM dd, yyyy')}
                  </Typography>
                </Box>
              )}
            </Paper>
          </Box>
          <Box>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Maximum Price
              </Typography>
              <Typography variant="h4" color="error">
                ${priceStats.maxPrice?.toFixed(2)}
              </Typography>
              {priceStats.maxPricePurchase && (
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mt: 1 }}>
                  <StationLogo station={priceStats.maxPricePurchase.fuelStation} size={20} />
                  <Typography variant="body2" color="textSecondary">
                    {priceStats.maxPricePurchase.fuelStation} -{' '}
                    {format(new Date(priceStats.maxPricePurchase.dateAndTime), 'MMM dd, yyyy')}
                  </Typography>
                </Box>
              )}
            </Paper>
          </Box>
        </Box>
      )}

      {/* Price Trends Chart */}
      {trends.length > 0 && (
        <Box sx={{ mb: 3 }}>
          <Box>
            <Paper sx={{ p: 3 }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                <Typography variant="h6">Price Trends</Typography>
                <ToggleButtonGroup
                  value={trendPeriod}
                  exclusive
                  onChange={(_, value) => value && setTrendPeriod(value)}
                  size="small"
                >
                  <ToggleButton value="daily">Daily</ToggleButton>
                  <ToggleButton value="monthly">Monthly</ToggleButton>
                  <ToggleButton value="yearly">Yearly</ToggleButton>
                </ToggleButtonGroup>
              </Box>
              <Line data={priceTrendsData} options={chartOptions} />
            </Paper>
          </Box>
        </Box>
      )}

      {/* Spending by Station */}
      {stationStats.length > 0 && (
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: { xs: '1fr', md: 'repeat(2, 1fr)' },
            gap: 3,
            mb: 3,
          }}
        >
          <Box>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Spending by Station
              </Typography>
              <Bar data={spendingByStationData} options={chartOptions} />
            </Paper>
          </Box>
          <Box>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Spending Distribution
              </Typography>
              <Doughnut data={spendingByStationData} options={chartOptions} />
            </Paper>
          </Box>
        </Box>
      )}

      {/* Monthly & Yearly Comparisons */}
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: { xs: '1fr', md: 'repeat(2, 1fr)' },
          gap: 3,
        }}
      >
        {monthlyStats.length > 0 && (
          <Box>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Monthly Spending
              </Typography>
              <Bar data={monthlySpendingData} options={chartOptions} />
            </Paper>
          </Box>
        )}
        {yearlyStats.length > 0 && (
          <Box>
            <Paper sx={{ p: 3 }}>
              <Typography variant="h6" gutterBottom>
                Yearly Spending
              </Typography>
              <Bar data={yearlySpendingData} options={chartOptions} />
            </Paper>
          </Box>
        )}
      </Box>
    </Container>
  );
};

export default AnalyticsDashboard;
