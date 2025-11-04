import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link, useLocation } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Box, Container } from '@mui/material';
import { LocalGasStation as GasIcon, ShowChart as ChartIcon, List as ListIcon } from '@mui/icons-material';
import AnalyticsDashboard from './components/AnalyticsDashboard';
import PurchaseList from './components/PurchaseList';
import PurchaseForm from './components/PurchaseForm';
import './App.css';

const Navigation: React.FC = () => {
  const location = useLocation();

  return (
    <AppBar position="static">
      <Toolbar>
        <GasIcon sx={{ mr: 2 }} />
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          Gas Tracker
        </Typography>
        <Box sx={{ display: 'flex', gap: 1 }}>
          <Button
            color="inherit"
            component={Link}
            to="/"
            startIcon={<ChartIcon />}
            variant={location.pathname === '/' ? 'outlined' : 'text'}
          >
            Dashboard
          </Button>
          <Button
            color="inherit"
            component={Link}
            to="/purchases"
            startIcon={<ListIcon />}
            variant={location.pathname === '/purchases' ? 'outlined' : 'text'}
          >
            Purchases
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

function App() {
  return (
    <Router>
      <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
        <Navigation />
        <Box component="main" sx={{ flexGrow: 1, bgcolor: 'background.default' }}>
          <Routes>
            <Route path="/" element={<AnalyticsDashboard />} />
            <Route path="/purchases" element={<PurchaseList />} />
            <Route path="/purchases/new" element={<PurchaseForm />} />
            <Route path="/purchases/edit/:id" element={<PurchaseForm />} />
          </Routes>
        </Box>
      </Box>
    </Router>
  );
}

export default App;