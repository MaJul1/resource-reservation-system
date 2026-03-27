FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore

ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef

RUN dotnet ef database update
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development
COPY --from=build /app/out ./
COPY --from=build /app/app.db ./

EXPOSE 5007
ENTRYPOINT [ "dotnet", "resource-reservation-system-backend.dll" ]
