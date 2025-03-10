import { Paper, Typography, Box, Alert } from '@mui/material';
import React from 'react';

const PaymentHistory: React.FC = () => {
  return (
    <Paper elevation={3} style={{ padding: '20px', maxWidth: '800px', margin: '0 auto' }}>
      <Typography variant="h4" gutterBottom>
        Payment History
      </Typography>
      
      <Alert severity="info" style={{ marginBottom: '20px' }}>
        This is a demo application. In a real application, this page would display a list of past payments.
      </Alert>
      
      <Box mt={2}>
        <Typography variant="body1">
          The Payment Gateway API currently only supports:
        </Typography>
        <ul>
          <li>Creating new payments via POST to /api/payments</li>
          <li>Retrieving payment details via GET to /api/payments/{'{id}'}</li>
        </ul>
        <Typography variant="body1" mt={2}>
          A full implementation would include endpoints to retrieve payment history and would display that information here.
        </Typography>
      </Box>
    </Paper>
  );
};

export default PaymentHistory; 