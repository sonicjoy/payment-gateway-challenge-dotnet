import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, MenuItem, Select, FormControl, InputLabel, Grid, Paper, Typography, Box, Alert } from '@mui/material';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import { paymentService } from '../services/api';
import { CurrencyEnum, PaymentStatus } from '../types';

// Validation schema
const PaymentSchema = Yup.object().shape({
  cardNumber: Yup.string()
    .required('Card number is required')
    .matches(/^\d{16}$/, 'Card number must be 16 digits'),
  expiryMonth: Yup.number()
    .required('Expiry month is required')
    .min(1, 'Invalid month')
    .max(12, 'Invalid month'),
  expiryYear: Yup.number()
    .required('Expiry year is required')
    .min(new Date().getFullYear(), 'Card has expired')
    .max(new Date().getFullYear() + 10, 'Expiry year too far in the future'),
  amount: Yup.number()
    .required('Amount is required')
    .positive('Amount must be positive'),
  currency: Yup.string()
    .required('Currency is required'),
  cvv: Yup.string()
    .required('CVV is required')
    .matches(/^\d{3,4}$/, 'CVV must be 3 or 4 digits'),
});

const PaymentForm: React.FC = () => {
  const navigate = useNavigate();
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const handleSubmit = async (values: any, { setSubmitting, resetForm }: any) => {
    try {
      setError(null);
      setSuccess(null);
      
      const response = await paymentService.createPayment({
        cardNumber: values.cardNumber,
        expiryMonth: values.expiryMonth,
        expiryYear: values.expiryYear,
        currency: values.currency,
        amount: values.amount,
        cvv: values.cvv,
      });

      if (response.id) {
        if (response.status === PaymentStatus.Authorized) {
          setSuccess('Payment authorized successfully!');
          setTimeout(() => {
            navigate(`/payment/${response.id}`);
          }, 2000);
        } else if (response.status === PaymentStatus.Declined) {
          setError('Payment was declined by the bank.');
        } else {
          setError('Payment was rejected. Please check your details.');
        }
      }
      
      resetForm();
    } catch (err: any) {
      setError(err.message || 'An error occurred while processing your payment.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Paper elevation={3} style={{ padding: '20px', maxWidth: '800px', margin: '0 auto' }}>
      <Typography variant="h4" gutterBottom>
        Make a Payment
      </Typography>
      
      {error && (
        <Alert severity="error" style={{ marginBottom: '20px' }}>
          {error}
        </Alert>
      )}
      
      {success && (
        <Alert severity="success" style={{ marginBottom: '20px' }}>
          {success}
        </Alert>
      )}
      
      <Formik
        initialValues={{
          cardNumber: '',
          expiryMonth: '',
          expiryYear: '',
          currency: CurrencyEnum.USD,
          amount: '',
          cvv: '',
        }}
        validationSchema={PaymentSchema}
        onSubmit={handleSubmit}
      >
        {({ errors, touched, isSubmitting, handleChange, values }) => (
          <Form>
            <Grid container spacing={3}>
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  id="cardNumber"
                  name="cardNumber"
                  label="Card Number"
                  value={values.cardNumber}
                  onChange={handleChange}
                  error={touched.cardNumber && Boolean(errors.cardNumber)}
                  helperText={touched.cardNumber && errors.cardNumber}
                  placeholder="1234567890123456"
                />
              </Grid>
              
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  id="expiryMonth"
                  name="expiryMonth"
                  label="Expiry Month"
                  type="number"
                  value={values.expiryMonth}
                  onChange={handleChange}
                  error={touched.expiryMonth && Boolean(errors.expiryMonth)}
                  helperText={touched.expiryMonth && errors.expiryMonth}
                  placeholder="MM"
                  inputProps={{ min: 1, max: 12 }}
                />
              </Grid>
              
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  id="expiryYear"
                  name="expiryYear"
                  label="Expiry Year"
                  type="number"
                  value={values.expiryYear}
                  onChange={handleChange}
                  error={touched.expiryYear && Boolean(errors.expiryYear)}
                  helperText={touched.expiryYear && errors.expiryYear}
                  placeholder="YYYY"
                  inputProps={{ min: new Date().getFullYear() }}
                />
              </Grid>
              
              <Grid item xs={6}>
                <FormControl fullWidth>
                  <InputLabel id="currency-label">Currency</InputLabel>
                  <Select
                    labelId="currency-label"
                    id="currency"
                    name="currency"
                    value={values.currency}
                    onChange={handleChange}
                    label="Currency"
                  >
                    <MenuItem value={CurrencyEnum.USD}>USD</MenuItem>
                    <MenuItem value={CurrencyEnum.EUR}>EUR</MenuItem>
                    <MenuItem value={CurrencyEnum.GBP}>GBP</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
              
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  id="amount"
                  name="amount"
                  label="Amount"
                  type="number"
                  value={values.amount}
                  onChange={handleChange}
                  error={touched.amount && Boolean(errors.amount)}
                  helperText={touched.amount && errors.amount}
                  inputProps={{ min: 0 }}
                />
              </Grid>
              
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  id="cvv"
                  name="cvv"
                  label="CVV"
                  value={values.cvv}
                  onChange={handleChange}
                  error={touched.cvv && Boolean(errors.cvv)}
                  helperText={touched.cvv && errors.cvv}
                  placeholder="123"
                />
              </Grid>
              
              <Grid item xs={12}>
                <Box display="flex" justifyContent="flex-end">
                  <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                    disabled={isSubmitting}
                  >
                    {isSubmitting ? 'Processing...' : 'Submit Payment'}
                  </Button>
                </Box>
              </Grid>
            </Grid>
          </Form>
        )}
      </Formik>
    </Paper>
  );
};

export default PaymentForm; 