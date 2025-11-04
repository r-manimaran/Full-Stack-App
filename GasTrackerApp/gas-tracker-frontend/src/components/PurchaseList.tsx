import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Container,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Button,
  Typography,
  Box,
  IconButton,
  Alert,
  Chip,
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon, Add as AddIcon } from '@mui/icons-material';
import { gasPurchasesApi } from '../services/api';
import { GasPurchase } from '../types';
import { format } from 'date-fns';

const PurchaseList: React.FC = () => {
  const navigate = useNavigate();
  const [purchases, setPurchases] = useState<GasPurchase[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadPurchases();
  }, []);

  const loadPurchases = async () => {
    try {
      setLoading(true);
      const data = await gasPurchasesApi.getAll();
      setPurchases(data);
    } catch (err) {
      setError('Failed to load purchases');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this purchase?')) {
      return;
    }

    try {
      await gasPurchasesApi.delete(id);
      loadPurchases();
    } catch (err) {
      setError('Failed to delete purchase');
    }
  };

  const handleEdit = (id: number) => {
    navigate(`/purchases/edit/${id}`);
  };

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4">Gas Purchases</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => navigate('/purchases/new')}
        >
          Add New Purchase
        </Button>
      </Box>

      {error && (
        <Alert severity="error" sx={{ mb: 2 }} onClose={() => setError(null)}>
          {error}
        </Alert>
      )}

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell><strong>Date & Time</strong></TableCell>
              <TableCell><strong>Station</strong></TableCell>
              <TableCell align="right"><strong>Price/Gallon</strong></TableCell>
              <TableCell align="right"><strong>Gallons</strong></TableCell>
              <TableCell align="right"><strong>Total Price</strong></TableCell>
              <TableCell align="center"><strong>Actions</strong></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={6} align="center">
                  Loading...
                </TableCell>
              </TableRow>
            ) : purchases.length === 0 ? (
              <TableRow>
                <TableCell colSpan={6} align="center">
                  No purchases found. Add your first purchase!
                </TableCell>
              </TableRow>
            ) : (
              purchases.map((purchase) => (
                <TableRow key={purchase.id} hover>
                  <TableCell>
                    {format(new Date(purchase.dateAndTime), 'MMM dd, yyyy HH:mm')}
                  </TableCell>
                  <TableCell>
                    <Chip label={purchase.fuelStation} color="primary" size="small" />
                  </TableCell>
                  <TableCell align="right">
                    ${purchase.pricePerGallon.toFixed(2)}
                  </TableCell>
                  <TableCell align="right">
                    {purchase.gallonsPurchased.toFixed(2)}
                  </TableCell>
                  <TableCell align="right">
                    <strong>${purchase.totalPrice.toFixed(2)}</strong>
                  </TableCell>
                  <TableCell align="center">
                    <IconButton
                      color="primary"
                      onClick={() => handleEdit(purchase.id)}
                      size="small"
                    >
                      <EditIcon />
                    </IconButton>
                    <IconButton
                      color="error"
                      onClick={() => handleDelete(purchase.id)}
                      size="small"
                    >
                      <DeleteIcon />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
};

export default PurchaseList;
