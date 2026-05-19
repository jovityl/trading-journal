# Trading Journal — Backend

> ASP.NET Core 10 API for an AI-powered options trading journal. Built with Clean Architecture and CQRS.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-10-512BD4)
![Auth0](https://img.shields.io/badge/Auth0-EB5424?logo=auth0&logoColor=white)
![Claude](https://img.shields.io/badge/Claude-Sonnet_4.5-D97706)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)

## ✨ Features

- 🔐 **JWT auth** via Auth0 — every endpoint protected, user-scoped data
- 📝 **Trades CRUD** — with chart + IBKR screenshot file upload
- 📊 **Dashboard aggregation** — P&L, win rate, discipline score, equity curve, daily P&L chart
- 🤖 **AI chart scoring** — Claude Sonnet 4.5 analyzes uploaded charts and scores 0-100
- 🏷 **Violation tags** — tag behavioral mistakes (FOMO, revenge trade, etc.) per trade
- 📈 **Discipline analytics** — violation tag frequency stats and clean trade rate
- 💬 **AI trade chat** — per-trade conversation with Claude using trade context
- ⚙️ **User settings** — daily loss/profit limits with dashboard alerts
- 🧪 **Seed/wipe endpoints** — admin-only, for local testing

## 🛠 Tech Stack

| Category | Technology |
|----------|-----------|
| Framework | ASP.NET Core (.NET 10) |
| Database | PostgreSQL + EF Core |
| Architecture | Clean Architecture (5 layers) |
| Pattern | CQRS via MediatR |
| Auth | Auth0 (JWT bearer) |
| AI | Anthropic Claude Sonnet 4.5 |
| Storage | Local filesystem (S3-ready interface) |
| Containerisation | Docker |

## 🏗 Architecture

```
TradingJournal.Api              ← Controllers, DI, middleware
TradingJournal.Application      ← MediatR handlers (commands & queries)
TradingJournal.Domain           ← Entities, repository interfaces
TradingJournal.Infrastructure   ← EF Core, repositories, external services
TradingJournal.Contract         ← DTOs, request/response models
```

Dependencies flow inward: Api → Application → Domain. Infrastructure implements Domain interfaces.

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL 14+
- Auth0 account with an API configured
- Anthropic API key (for AI scoring)

### Setup

1. **Configure user secrets**
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=tradingjournal;Username=postgres;Password=YOUR_PASSWORD" --project TradingJournal.Api

   dotnet user-secrets set "Auth0:Domain" "your-tenant.auth0.com" --project TradingJournal.Api
   dotnet user-secrets set "Auth0:Audience" "https://trading-journal-api" --project TradingJournal.Api

   dotnet user-secrets set "Anthropic:ApiKey" "sk-ant-..." --project TradingJournal.Api
   ```

2. **Apply database migrations**
   ```bash
   dotnet ef database update --project TradingJournal.Infrastructure --startup-project TradingJournal.Api
   ```

3. **Run the API**
   ```bash
   dotnet run --project TradingJournal.Api --launch-profile https
   ```

4. API available at [https://localhost:7160](https://localhost:7160) — Swagger at `/swagger`

### Docker (full stack)

Runs the API, PostgreSQL, and frontend together.

**Before you start you'll need:**
- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed
- A free [Auth0](https://auth0.com) account — create an API + a SPA application
- An [Anthropic](https://console.anthropic.com) API key — required for AI chart scoring
- An [OpenRouter](https://openrouter.ai) API key — required for AI trade chat

1. **Create a `.env` file** from the template
   ```bash
   # Windows
   copy .env.example .env
   # Mac/Linux
   cp .env.example .env
   # then fill in your real values
   ```

2. **Start everything**
   ```bash
   docker-compose up --build
   ```

- Frontend → `http://localhost:3000`
- API → `http://localhost:8080`
- Swagger → `http://localhost:8080/swagger`

The API auto-migrates the database on startup. No manual migration step needed.

## 🔌 Key Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/v1/dashboard` | Dashboard stats + charts + discipline analytics |
| GET | `/api/v1/trades` | List trades (ticker, type, strategy, date, violation tag filters) |
| GET | `/api/v1/trades/{id}` | Trade detail |
| POST | `/api/v1/trades` | Log new trade (files + AI scoring + violation tags) |
| DELETE | `/api/v1/trades/{id}` | Delete a trade |
| POST | `/api/v1/trades/{id}/chat` | Chat with Claude about a trade |
| GET | `/api/v1/users/me` | Current user info |
| PUT | `/api/v1/users/limits` | Update daily limits |
| POST | `/api/v1/trades/seed` | Seed test data (admin only) |
| DELETE | `/api/v1/trades/all` | Delete all trades (admin only) |

All endpoints require `Authorization: Bearer <jwt>` header.

## 🏷 Discipline System

Each trade gets two scores:

- **Discipline Score** — computed from violation tag count: 0 tags = 100, 1 = 70, 2 = 40, 3+ = 10. Objective and tamper-proof.
- **AI Score** — Claude analyzes the uploaded chart image and scores 0-100 based on entry/exit quality.

Available violation tags: `Revenge Trade`, `FOMO Entry`, `Oversized Position`, `Early Exit`, `Late Exit`, `Chased Entry`, `No Clear Setup`, `Broke Profit Target`, `Overtraded`.

## 🔗 Related

- [Frontend repository](https://github.com/jovityl/trading-journal-frontend)
