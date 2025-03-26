FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем все .csproj файлы из всех проектов
COPY KanbanApp.Core/KanbanApp.Core.csproj KanbanApp.Core/
COPY KanbanApp.DataAccess/KanbanApp.DataAccess.csproj KanbanApp.DataAccess/
COPY KanbanApp.Application/KanbanApp.Application.csproj KanbanApp.Application/
COPY KanbanApp.API/KanbanApp.API.csproj KanbanApp.API/

# Восстанавливаем зависимости
RUN dotnet restore KanbanApp.API/KanbanApp.API.csproj

# Копируем остальные файлы
COPY . .
WORKDIR /src/KanbanApp.API
RUN dotnet build KanbanApp.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish KanbanApp.API.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KanbanApp.API.dll"]