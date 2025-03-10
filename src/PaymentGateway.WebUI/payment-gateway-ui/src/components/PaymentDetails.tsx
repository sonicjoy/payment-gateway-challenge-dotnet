import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
  Paper, 
  Typography, 
  Box, 
  Grid, 
  Chip, 
  Button, 
  CircularProgress, 
  Alert 
} from '@mui/material';
import { PaymentResponse, PaymentStatus } from '../types';
import { paymentService } from '../services/api';

const PaymentDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [payment, setPayment] = useState<PaymentResponse | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPayment = async () => {
      if (!id) {
        setError('Payment ID is missing');
        setLoading(false);
        return;
      }

      try {
        const data = await paymentService.getPayment(id);
        setPayment(data);
      } catch (err: any) {
        setError(err.message || 'Failed to fetch payment details');
      } finally {
        setLoading(false);
      }
    };

    fetchPayment();
  }, [id]);

  const getStatusColor = (status: PaymentStatus) => {
    switch (status) {
      case PaymentStatus.Authorized:
        return 'success';
      case PaymentStatus.Declined:
        return 'warning';
      case PaymentStatus.Rejected:
        return 'error';
      default:
        return 'default';
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleString();
  };

  if (loading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="300px">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" style={{ marginTop: '20px' }}>
        {error}
      </Alert>
    );
  }

  if (!payment) {
    return (
      <Alert severity="info" style={{ marginTop: '20px' }}>
        No payment details found.
      </Alert>
    );
  }

  return (
    <Paper elevation={3} style={{ padding: '20px', maxWidth: '800px', margin: '0 auto' }}>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h4">Payment Details</Typography>
        <Chip 
          label={payment.status} 
          color={getStatusColor(payment.status) as any}
          variant="outlined"
        />
      </Box>

      <Grid container spacing={3}>
        <Grid item xs={12} sm={6}>
          <Typography variant="subtitle2" color="textSecondary">Payment ID</Typography>
          <Typography variant="body1">{payment.id}</Typography>
        </Grid>

        <Grid item xs={12} sm={6}>
          <Typography variant="subtitle2" color="textSecondary">Date</Typography>
          <Typography variant="body1">{formatDate(payment.createdAt)}</Typography>
        </Grid>

        <Grid item xs={12} sm={6}>
          <Typography variant="subtitle2" color="textSecondary">Card Number</Typography>
          <Typography variant="body1">**** **** **** {payment.cardNumberLastFour}</Typography>
        </Grid>

        <Grid item xs={12} sm={6}>
          <Typography variant="subtitle2" color="textSecondary">Expiry Date</Typography>
          <Typography variant="body1">{payment.expiryMonth}/{payment.expiryYear}</Typography>
        </Grid>

        <Grid item xs={12} sm={6}>
          <Typography variant="subtitle2" color="textSecondary">Amount</Typography>
          <Typography variant="body1">{payment.amount} {payment.currency}</Typography>
        </Grid>

        <Grid item xs={12} sm={6}>
          <Typography variant="subtitle2" color="textSecondary">Status</Typography>
          <Typography variant="body1">{payment.status}</Typography>
        </Grid>
      </Grid>

      <Box mt={4} display="flex" justifyContent="flex-end">
        <Button 
          variant="outlined" 
          color="primary" 
          onClick={() => navigate('/')}
        >
          Make Another Payment
        </Button>
      </Box>
    </Paper>
  );
};

export default PaymentDetails; 