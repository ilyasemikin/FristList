FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

EXPOSE 80
EXPOSE 443

COPY ./FristList.Service.Data/FristList.Service.Data.csproj ./FristList.Service.Data/
COPY ./FristList.Service.PublicApi/FristList.Service.PublicApi.csproj ./FristList.Service.PublicApi/

WORKDIR /app/FristList.Service.PublicApi
RUN dotnet restore

WORKDIR /app
COPY ./FristList.Service.Data/ ./FristList.Service.Data/
COPY ./FristList.Service.PublicApi/ ./FristList.Service.PublicApi/

WORKDIR /app/FristList.Service.PublicApi
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/FristList.Service.PublicApi/out ./
ENTRYPOINT [ "dotnet", "FristList.Service.PublicApi.dll" ]
