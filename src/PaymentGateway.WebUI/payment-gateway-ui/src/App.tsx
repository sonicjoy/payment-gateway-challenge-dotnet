import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import './App.css';

// Import components we'll create
import PaymentForm from './components/PaymentForm';
import PaymentDetails from './components/PaymentDetails';
import PaymentHistory from './components/PaymentHistory';

function App() {
  return (
    <Router>
      <div className="App">
        <header className="App-header">
          <h1>Payment Gateway Demo</h1>
          <nav>
            <ul style={{ display: 'flex', listStyle: 'none', gap: '20px' }}>
              <li>
                <Link to="/" style={{ color: 'white' }}>Make Payment</Link>
              </li>
              <li>
                <Link to="/history" style={{ color: 'white' }}>Payment History</Link>
              </li>
            </ul>
          </nav>
        </header>
        <main style={{ padding: '20px' }}>
          <Routes>
            <Route path="/" element={<PaymentForm />} />
            <Route path="/payment/:id" element={<PaymentDetails />} />
            <Route path="/history" element={<PaymentHistory />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
