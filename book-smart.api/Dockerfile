FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["book-smart.api/book-smart.api.csproj", "book-smart.api/"]
RUN dotnet restore "book-smart.api/book-smart.api.csproj"
COPY . .
WORKDIR "/src/book-smart.api"
RUN dotnet build "book-smart.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "book-smart.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "book-smart.api.dll"]
