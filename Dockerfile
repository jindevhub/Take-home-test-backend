# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY *.sln .
COPY Take-home-test-backend/*.csproj ./Take-home-test-backend/
COPY Take-home-test-backend.UnitTests/*.csproj ./Take-home-test-backend.UnitTests/
RUN dotnet restore

# Copy the remaining files and build the application
COPY . .
WORKDIR /app/Take-home-test-backend
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/Take-home-test-backend/out ./
# Expose port 5000
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "Take-home-test-backend.dll"]
