# Payment Gateway UI

This is a React-based UI for the Payment Gateway API. It allows users to submit payment requests and view payment details.

## Features

- Submit payment requests with credit card information
- View payment details after submission
- Responsive design using Material-UI

## Prerequisites

- Node.js (v14 or higher)
- npm (v6 or higher)
- .NET 8.0 SDK (for running the API)

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Install dependencies:

```bash
npm install
```

4. Start both the API and the React app using the provided PowerShell script:

```bash
./start-dev.ps1
```

This will:
- Start the Payment Gateway API on http://localhost:5000
- Start the React app on http://localhost:3000

5. Open [http://localhost:3000](http://localhost:3000) in your browser

## Running Separately

### API

To run the API separately:

```bash
cd ../../PaymentGateway.Api
dotnet run
```

### React App

To run the React app separately:

```bash
npm start
```

## API Configuration

The application is configured to connect to the Payment Gateway API at `http://localhost:5000/api`. If your API is running on a different URL, you can update the `API_URL` constant in `src/services/api.ts`.

## Available Scripts

In the project directory, you can run:

- `npm start`: Runs the app in development mode
- `npm test`: Launches the test runner
- `npm run build`: Builds the app for production
- `npm run eject`: Ejects from create-react-app

## Project Structure

- `src/components`: React components
- `src/services`: API services
- `src/types`: TypeScript interfaces and types

## Technologies Used

- React
- TypeScript
- Material-UI
- Formik (form handling)
- Yup (form validation)
- Axios (API requests)
- React Router (routing)
