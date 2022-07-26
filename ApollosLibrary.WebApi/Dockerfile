#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["ApollosLibrary.WebApi/ApollosLibrary.WebApi.csproj", "ApollosLibrary.WebApi/"]
COPY ["ApollosLibrary.Domain/ApollosLibrary.Domain.csproj", "ApollosLibrary.Domain/"]
COPY ["ApollosLibrary.Infrastructure/ApollosLibrary.Infrastructure.csproj", "ApollosLibrary.Infrastructure/"]
COPY ["ApollosLibrary.Application/ApollosLibrary.Application.csproj", "ApollosLibrary.Application/"]
COPY ["ApollosLibrary.Contracts.UnitOfWork/ApollosLibrary.UnitOfWork.Contracts.csproj", "ApollosLibrary.Contracts.UnitOfWork/"]
COPY ["ApollosLibrary.DataLayer.Contracts/ApollosLibrary.DataLayer.Contracts.csproj", "ApollosLibrary.DataLayer.Contracts/"]
COPY ["ApollosLibrary.UnitOfWork/ApollosLibrary.UnitOfWork.csproj", "ApollosLibrary.UnitOfWork/"]
COPY ["ApollosLibrary.DataLayer/ApollosLibrary.DataLayer.csproj", "ApollosLibrary.DataLayer/"]
RUN dotnet restore "ApollosLibrary.WebApi/ApollosLibrary.WebApi.csproj"
COPY . .
WORKDIR "/src/ApollosLibrary.WebApi"
RUN dotnet build "ApollosLibrary.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApollosLibrary.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApollosLibrary.WebApi.dll"]