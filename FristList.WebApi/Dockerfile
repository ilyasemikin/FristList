FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /app

EXPOSE 80
EXPOSE 443

COPY ./FristList.Dto/*.csproj ./FristList.Dto/
COPY ./FristList.Models/*.csproj ./FristList.Models/
COPY ./FristList.Services/*.csproj ./FristList.Services/
COPY ./FristList.WebApi/*.csproj ./FristList.WebApi/

WORKDIR /app/FristList.WebApi/
RUN dotnet restore

WORKDIR /app
COPY ./FristList.Dto/. ./FristList.Dto/
COPY ./FristList.Models/. ./FristList.Models/
COPY ./FristList.Services/. ./FristList.Services/
COPY ./FristList.WebApi/. ./FristList.WebApi/

WORKDIR /app/FristList.WebApi/
RUN dotnet build -c Debug --no-restore -o out

FROM mcr.microsoft.com/dotnet/aspnet AS runtime
WORKDIR /app

COPY --from=build /app/FristList.WebApi/out ./
ENTRYPOINT [ "dotnet", "FristList.WebApi.dll" ]
