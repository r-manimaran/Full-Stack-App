# Gas Purchase Tracker - Full Stack Application

A full-stack web application for tracking gas purchases with comprehensive analytics and visualizations.

## Features

- **Manual Entry Form**: Add gas purchases with auto-calculated total price
- **Purchase Management**: View, edit, and delete gas purchases
- **Analytics Dashboard**: Comprehensive analytics with multiple charts and visualizations:
  - Summary statistics (total spent, total gallons, average price, etc.)
  - Min/Max price tracking with dates
  - Price trends over time (daily/monthly/yearly)
  - Spending breakdown by station
  - Monthly and yearly spending comparisons
- **SQLite Database**: Lightweight, file-based database for easy data storage

## Tech Stack

### Backend
- ASP.NET Core 10.0 Web API
- Entity Framework Core
- SQLite Database
- C# 10.0

### Frontend
- React 18 with TypeScript
- Material-UI (MUI) for modern UI components
- Chart.js & React-Chartjs-2 for data visualizations
- React Router for navigation
- Axios for API communication

## Prerequisites

- .NET 10.0 SDK or later
- Node.js 16+ and npm
- Visual Studio 2022 or VS Code (optional, for development)

## Setup Instructions

### Backend Setup

1. Navigate to the API directory:
   ```bash
   cd GasTrackerApi
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

   The API will be available at:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:7000`
   - Swagger UI: `https://localhost:7000/swagger` (in development mode)

   The SQLite database (`GasTracker.db`) will be automatically created on first run.

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd gas-tracker-frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

   The frontend will be available at `http://localhost:3000`

4. (Optional) Configure API URL:
   Create a `.env` file in `gas-tracker-frontend` directory:
   ```
   REACT_APP_API_URL=http://localhost:5000
   ```

## Usage

1. **Start the Backend**: Run `dotnet run` in the `GasTrackerApi` directory
2. **Start the Frontend**: Run `npm start` in the `gas-tracker-frontend` directory
3. **Open Browser**: Navigate to `http://localhost:3000`

### Adding a Purchase

1. Click "Add New Purchase" button
2. Fill in the form:
   - Price per Gallon
   - Gallons Purchased
   - Date and Time
   - Fuel Station (select from dropdown or enter custom)
3. The total price will be automatically calculated
4. Click "Save"

### Viewing Analytics

1. Navigate to the Dashboard (default home page)
2. View summary statistics, price trends, and spending breakdowns
3. Switch between daily, monthly, and yearly views for trends

### Managing Purchases

1. Navigate to "Purchases" from the navigation bar
2. View all purchases in a table format
3. Edit or delete purchases using the action buttons

## API Endpoints

### Gas Purchases
- `GET /api/gaspurchases` - Get all purchases
- `GET /api/gaspurchases/{id}` - Get purchase by ID
- `POST /api/gaspurchases` - Create new purchase
- `PUT /api/gaspurchases/{id}` - Update purchase
- `DELETE /api/gaspurchases/{id}` - Delete purchase

### Analytics
- `GET /api/analytics/summary` - Get summary statistics
- `GET /api/analytics/pricestats` - Get min/max price statistics
- `GET /api/analytics/trends?period={daily|monthly|yearly}` - Get price trends
- `GET /api/analytics/bystation` - Get spending by station
- `GET /api/analytics/monthly` - Get monthly statistics
- `GET /api/analytics/yearly` - Get yearly statistics

## Database

The SQLite database file (`GasTracker.db`) is created automatically in the `GasTrackerApi` directory. The database contains a single table:

### GasPurchases Table
- `Id` (int, Primary Key)
- `PricePerGallon` (decimal)
- `GallonsPurchased` (decimal)
- `DateAndTime` (DateTime)
- `TotalPrice` (decimal)
- `FuelStation` (string)

## Project Structure

```
GasTrackerApp/
├── GasTrackerApi/              # Backend API
│   ├── Controllers/
│   │   ├── GasPurchasesController.cs
│   │   └── AnalyticsController.cs
│   ├── Models/
│   │   └── GasPurchase.cs
│   ├── Data/
│   │   └── GasTrackerDbContext.cs
│   ├── Services/
│   │   └── GasPurchaseService.cs
│   ├── Program.cs
│   └── appsettings.json
├── gas-tracker-frontend/       # React Frontend
│   ├── src/
│   │   ├── components/
│   │   │   ├── PurchaseForm.tsx
│   │   │   ├── PurchaseList.tsx
│   │   │   └── AnalyticsDashboard.tsx
│   │   ├── services/
│   │   │   └── api.ts
│   │   ├── types/
│   │   │   └── index.ts
│   │   ├── App.tsx
│   │   └── index.tsx
│   └── package.json
└── README.md
```

## Future Enhancements

- Bill scan/OCR functionality for automatic entry
- Additional fields (vehicle, odometer, payment method)
- Export data to CSV/Excel
- User authentication and multi-user support
- Mobile app version

## Troubleshooting

### CORS Issues
If you encounter CORS errors, ensure:
- The backend is running before starting the frontend
- The API URL in the frontend matches the backend URL
- CORS is properly configured in `Program.cs`

### Database Issues
If the database is not created:
- Ensure the `GasTrackerApi` directory has write permissions
- Check that Entity Framework Core is properly installed
- The database will be created on first API run

### Port Conflicts
If ports 3000, 5000, or 7000 are already in use:
- For frontend: Set `PORT` environment variable or modify `package.json`
- For backend: Modify `launchSettings.json` or use `--urls` parameter

## License

This project is for personal use.
