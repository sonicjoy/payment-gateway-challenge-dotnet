export enum PaymentStatus {
  Rejected = 'Rejected',
  Authorized = 'Authorized',
  Declined = 'Declined'
}

export enum CurrencyEnum {
  USD = 'USD',
  EUR = 'EUR',
  GBP = 'GBP'
}

export interface PaymentRequest {
  cardNumber: string;
  expiryMonth: number;
  expiryYear: number;
  currency: CurrencyEnum;
  amount: number;
  cvv: string;
}

export interface PaymentResponse {
  id?: string;
  status: PaymentStatus;
  cardNumberLastFour?: string;
  expiryMonth?: number;
  expiryYear?: number;
  currency: CurrencyEnum;
  amount?: number;
  createdAt: string;
} 