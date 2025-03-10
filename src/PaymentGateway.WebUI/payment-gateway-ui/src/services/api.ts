import axios from 'axios';
import { PaymentRequest, PaymentResponse } from '../types';

// Base URL for the API
const API_URL = 'http://localhost:5000/api'; // Adjust this to match your API URL

// Create axios instance
const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// API functions
export const paymentService = {
  // Create a new payment
  createPayment: async (paymentData: PaymentRequest): Promise<PaymentResponse> => {
    try {
      const response = await api.post('/payments', paymentData);
      return response.data;
    } catch (error) {
      console.error('Error creating payment:', error);
      throw error;
    }
  },

  // Get payment details by ID
  getPayment: async (id: string): Promise<PaymentResponse> => {
    try {
      const response = await api.get(`/payments/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching payment with ID ${id}:`, error);
      throw error;
    }
  },
}; 