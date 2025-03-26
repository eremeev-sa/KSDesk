# Используем официальный образ .NET для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY KanbanApp.API.csproj ./
RUN dotnet restore KanbanApp.API.csproj

# Копируем остальной код и собираем проект
COPY . ./
RUN dotnet publish -c Release -o out

# Используем образ для выполнения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "KanbanApp.API.dll"]