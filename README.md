# LearnPath — Graph-Based Learning Platform

A full-stack educational platform built with **ASP.NET Core** (backend) and **React + TypeScript** (frontend), featuring graph-based learning paths, classrooms, progress tracking, analytics, community features, and AI recommendations.

## Architecture

```
Frontend (React + Redux Toolkit)
        ↓
    Axios API Layer
        ↓
ASP.NET Core Controllers
        ↓
    Service Layer (Business Logic)
        ↓
    Repository Layer
        ↓
    Entity Framework Core
        ↓
    SQL Server / InMemory Database
```

## Tech Stack

### Backend
- **ASP.NET Core 8.0** Web API
- **Entity Framework Core** (Code-First)
- **ASP.NET Identity** + JWT Authentication
- **SQL Server** (production) / **InMemory DB** (development)
- Repository Pattern + Service Layer

### Frontend
- **React 19** + **TypeScript**
- **Vite** (build tool)
- **Redux Toolkit** (state management)
- **Material UI** (component library)
- **React Router v7** (routing)
- **React Hook Form** + **Yup** (form validation)
- **Axios** (HTTP client)
- **Recharts** (charts/analytics)

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js 18+ and npm
- SQL Server (optional, uses InMemory by default)

### Backend Setup
```bash
cd backend
dotnet restore
dotnet build
dotnet run
```
The API runs at `http://localhost:5000` with Swagger UI at `/swagger`.

### Frontend Setup
```bash
cd frontend
npm install
npm run dev
```
The app runs at `http://localhost:5173`.

### Environment Configuration

**Backend** (`backend/appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=LearnPathDb;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "Secret": "your-256-bit-secret-key-here",
    "Issuer": "LearnPath.API",
    "Audience": "LearnPath.Client",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

**Frontend** (`.env`):
```
VITE_API_URL=http://localhost:5000/api/v1
```

## Project Structure

### Backend
```
backend/
├── Controllers/          # API endpoints
├── Services/             # Business logic
├── Repositories/         # Data access
├── Entities/             # EF Core models
├── DTOs/                 # Data transfer objects
├── Interfaces/           # Contracts
├── Configurations/       # EF Fluent API
├── Authentication/       # JWT + Identity
├── Middleware/            # Exception, Logging
├── Algorithms/           # Graph (DAG), Recommendations
├── Data/                 # DbContext, Seeders
├── Responses/            # API response wrappers
├── Helpers/              # Utilities
└── Program.cs            # App entry point
```

### Frontend
```
frontend/src/
├── app/                  # Bootstrap (store, provider, router)
├── components/           # Reusable UI components
├── pages/                # Route pages
├── layouts/              # Layout wrappers
├── routes/               # Route definitions + guards
├── redux/                # Store + slices
├── services/             # Axios API layer
├── hooks/                # Custom hooks
├── utils/                # Helper functions
├── types/                # TypeScript types
├── validations/          # Yup schemas
├── constants/            # App constants
├── theme/                # MUI theme
├── graph/                # DAG algorithms
└── main.tsx              # Entry point
```

## API Endpoints

| Module | Route | Description |
|--------|-------|-------------|
| Auth | `POST /api/v1/auth/register` | User registration |
| Auth | `POST /api/v1/auth/login` | User login |
| Auth | `POST /api/v1/auth/refresh-token` | Token refresh |
| Paths | `GET /api/v1/paths` | Browse public paths |
| Paths | `POST /api/v1/paths` | Create learning path |
| Paths | `POST /api/v1/paths/:id/modules` | Add module to path |
| Classrooms | `POST /api/v1/classrooms` | Create classroom |
| Classrooms | `POST /api/v1/classrooms/join` | Join via code |
| Analytics | `GET /api/v1/analytics/dashboard` | User dashboard |
| Analytics | `POST /api/v1/analytics/progress/:moduleId/complete` | Complete module |
| Community | `POST /api/v1/community/comments` | Add comment |
| Community | `POST /api/v1/community/ratings/:pathId` | Rate path |
| Admin | `GET /api/v1/admin/stats` | Platform statistics |

## Key Features

- **Graph-Based Learning**: Modules organized as DAGs with prerequisite dependencies
- **Progress Tracking**: Module completion unlocks dependent modules automatically
- **Classrooms**: Instructors create classrooms with join codes, assign paths and assignments
- **JWT Auth**: Token-based authentication with refresh token rotation
- **Role-Based Access**: Admin, Instructor, and Learner roles
- **Community**: Comments, ratings, and notifications
- **AI Recommendations**: Recommendation engine based on user activity and interests
- **Analytics Dashboard**: Progress metrics, streaks, and completion rates
- **Responsive UI**: Material UI with mobile-first dashboard layout

## Default Credentials

After initial setup, an admin user is seeded:
- **Email**: `admin@learnpath.com`
- **Password**: `Admin@123!`
