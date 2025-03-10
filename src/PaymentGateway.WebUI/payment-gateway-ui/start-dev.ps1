# Start the API in a new PowerShell window
Start-Process powershell -ArgumentList "-Command", "cd ../../PaymentGateway.Api && dotnet run"

# Wait for the API to start
Write-Host "Starting API... Please wait for a few seconds"
Start-Sleep -Seconds 5

# Start the React app
Write-Host "Starting React app..."
npm start 