# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files for optimal caching
COPY ["UvaTea.slnx", "./"]
COPY ["UverTeaServerApp/UverTeaServerApp.csproj", "UverTeaServerApp/"]
COPY ["tests/UverTeaServerApp.UnitTests/UverTeaServerApp.UnitTests.csproj", "tests/UverTeaServerApp.UnitTests/"]

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the main application
WORKDIR "/src/UverTeaServerApp"
RUN dotnet build "UverTeaServerApp.csproj" -c Release -o /app/build

# Stage 2: Publish stage
FROM build AS publish
RUN dotnet publish "UverTeaServerApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UverTeaServerApp.dll"]