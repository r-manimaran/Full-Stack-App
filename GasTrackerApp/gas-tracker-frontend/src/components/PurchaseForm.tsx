import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Container,
  Paper,
  TextField,
  Button,
  Typography,
  Box,
  MenuItem,
  Alert,
} from '@mui/material';
import { gasPurchasesApi } from '../services/api';
import { GasPurchase } from '../types';

const COMMON_STATIONS = ['BJs', 'Costco', 'Circle K', 'Mobil', 'Shell', 'BP', 'Chevron', 'Exxon', 'Other'];

const PurchaseForm: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  const [formData, setFormData] = useState({
    pricePerGallon: '',
    gallonsPurchased: '',
    dateAndTime: new Date().toISOString().slice(0, 16),
    fuelStation: '',
  });
  const [totalPrice, setTotalPrice] = useState(0);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (isEdit && id) {
      loadPurchase(parseInt(id));
    }
  }, [id, isEdit]);

  useEffect(() => {
    const price = parseFloat(formData.pricePerGallon) || 0;
    const gallons = parseFloat(formData.gallonsPurchased) || 0;
    setTotalPrice(price * gallons);
  }, [formData.pricePerGallon, formData.gallonsPurchased]);

  const loadPurchase = async (purchaseId: number) => {
    try {
      setLoading(true);
      const purchase = await gasPurchasesApi.getById(purchaseId);
      setFormData({
        pricePerGallon: purchase.pricePerGallon.toString(),
        gallonsPurchased: purchase.gallonsPurchased.toString(),
        dateAndTime: new Date(purchase.dateAndTime).toISOString().slice(0, 16),
        fuelStation: purchase.fuelStation,
      });
    } catch (err) {
      setError('Failed to load purchase');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    setError(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!formData.pricePerGallon || !formData.gallonsPurchased || !formData.fuelStation) {
      setError('Please fill in all required fields');
      return;
    }

    try {
      setLoading(true);
      const purchaseData = {
        pricePerGallon: parseFloat(formData.pricePerGallon),
        gallonsPurchased: parseFloat(formData.gallonsPurchased),
        dateAndTime: new Date(formData.dateAndTime).toISOString(),
        fuelStation: formData.fuelStation,
        totalPrice: totalPrice,
      };

      if (isEdit && id) {
        await gasPurchasesApi.update(parseInt(id), purchaseData);
      } else {
        await gasPurchasesApi.create(purchaseData);
      }

      navigate('/purchases');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to save purchase');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h4" gutterBottom>
          {isEdit ? 'Edit' : 'Add New'} Gas Purchase
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <Box
            sx={{
              display: 'grid',
              gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)' },
              gap: 3,
            }}
          >
            <Box>
              <TextField
                fullWidth
                label="Price per Gallon ($)"
                name="pricePerGallon"
                type="number"
                inputProps={{ step: '0.01' }}
                value={formData.pricePerGallon}
                onChange={handleChange}
                required
                disabled={loading}
              />
            </Box>

            <Box>
              <TextField
                fullWidth
                label="Gallons Purchased"
                name="gallonsPurchased"
                type="number"
                inputProps={{ step: '0.01' }}
                value={formData.gallonsPurchased}
                onChange={handleChange}
                required
                disabled={loading}
              />
            </Box>

            <Box>
              <TextField
                fullWidth
                label="Date and Time"
                name="dateAndTime"
                type="datetime-local"
                value={formData.dateAndTime}
                onChange={handleChange}
                required
                disabled={loading}
                InputLabelProps={{
                  shrink: true,
                }}
              />
            </Box>

            <Box>
              <TextField
                fullWidth
                select
                label="Fuel Station"
                name="fuelStation"
                value={formData.fuelStation}
                onChange={handleChange}
                required
                disabled={loading}
              >
                {COMMON_STATIONS.map((station) => (
                  <MenuItem key={station} value={station}>
                    {station}
                  </MenuItem>
                ))}
              </TextField>
            </Box>

            <Box sx={{ gridColumn: { xs: '1 / -1', sm: '1 / -1' } }}>
              <Box
                sx={{
                  p: 2,
                  bgcolor: 'primary.light',
                  borderRadius: 1,
                  textAlign: 'center',
                }}
              >
                <Typography variant="h6" color="primary.contrastText">
                  Total Price: ${totalPrice.toFixed(2)}
                </Typography>
              </Box>
            </Box>

            <Box sx={{ gridColumn: { xs: '1 / -1', sm: '1 / -1' } }}>
              <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                <Button
                  variant="outlined"
                  onClick={() => navigate('/purchases')}
                  disabled={loading}
                >
                  Cancel
                </Button>
                <Button
                  type="submit"
                  variant="contained"
                  disabled={loading}
                >
                  {loading ? 'Saving...' : isEdit ? 'Update' : 'Save'}
                </Button>
              </Box>
            </Box>
          </Box>
        </form>
      </Paper>
    </Container>
  );
};

export default PurchaseForm;
