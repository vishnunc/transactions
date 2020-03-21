FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
RUN apk --no-cache add wget


WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.10 AS build
COPY . .
WORKDIR /Transactions
RUN dotnet build "Transactions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Transactions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transactions.dll"]
