# ============================================================
# Stage 1: base - runtime image
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app

# ============================================================
# Stage 2: build - restore and compile
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY Fintazz.Backend.slnx .
COPY Fintazz.Domain/Fintazz.Domain.csproj Fintazz.Domain/
COPY Fintazz.Application/Fintazz.Application.csproj Fintazz.Application/
COPY Fintazz.Infrastructure/Fintazz.Infrastructure.csproj Fintazz.Infrastructure/
COPY Fintazz.Api/Fintazz.Api.csproj Fintazz.Api/
COPY Fintazz.Worker/Fintazz.Worker.csproj Fintazz.Worker/

RUN dotnet restore Fintazz.Backend.slnx

COPY . .

RUN dotnet build Fintazz.Backend.slnx -c Release --no-restore

# ============================================================
# Stage 3: publish
# ============================================================
FROM build AS publish

RUN dotnet publish Fintazz.Api/Fintazz.Api.csproj \
    -c Release --no-build -o /app/publish/api

RUN dotnet publish Fintazz.Worker/Fintazz.Worker.csproj \
    -c Release --no-build -o /app/publish/worker

# ============================================================
# Stage 4: api
# ============================================================
FROM base AS api
WORKDIR /app

COPY --from=publish /app/publish/api .

USER app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Fintazz.Api.dll"]

# ============================================================
# Stage 5: worker
# ============================================================
FROM base AS worker
WORKDIR /app

COPY --from=publish /app/publish/worker .

USER app

ENTRYPOINT ["dotnet", "Fintazz.Worker.dll"]
