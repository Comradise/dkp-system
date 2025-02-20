# Use the official .NET SDK 8.0 image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG PACKAGE_STORAGE_USER
ARG PACKAGE_STORAGE_TOKEN

# Set the working directory inside the container
WORKDIR /app

# Copy the project files to the container
COPY . .

# Add nuget source
RUN dotnet nuget add source "https://gl-1.2pp.dev/api/v4/projects/1144/packages/nuget/index.json" --name PackageStorage --username $PACKAGE_STORAGE_USER --password $PACKAGE_STORAGE_TOKEN --store-password-in-clear-text

# Restore dependencies
RUN dotnet restore

# Build the application and publish the output to the "publish" directory
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET runtime 8.0 image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published files from the build container
COPY --from=build /app/publish .

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "dkp_system.dll"]