# Airbnb Clone

A complete starter Airbnb-style application using:

- ASP.NET Core 8 Web API
- Entity Framework Core 8
- SQL Server / LocalDB
- JWT authentication
- BCrypt password hashing
- React + TypeScript + Vite
- Axios
- React Router

## Demo accounts

Host:
- Email: `host@airbnb.local`
- Password: `Password123!`

Guest:
- Email: `guest@airbnb.local`
- Password: `Password123!`

## Run the backend

Open `Backend/Airbnb.sln` in Visual Studio 2022.

Or:

```powershell
cd Backend
dotnet restore
dotnet build
dotnet run --project Airbnb.Api
```

The API creates the database automatically with `EnsureCreated()` on startup.

If your API uses a different HTTPS port, update:

`Frontend/src/api/axios.ts`

or create:

`Frontend/.env`

with:

```text
VITE_API_URL=https://localhost:7001/api
```

## Run the frontend

```powershell
cd Frontend
npm install
npm run dev
```

Open the Vite URL shown in the terminal.

## Important production notes

This project is intentionally configured for local development. Before production:

1. Replace the JWT key with a strong secret stored outside source control.
2. Use EF Core migrations instead of `EnsureCreated()`.
3. Configure a real production database.
4. Restrict CORS to the actual frontend origin.
5. Add payment processing, email verification, password reset, image upload/storage, rate limiting, and stronger authorization rules.
