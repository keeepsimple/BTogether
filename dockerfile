FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.17-amd64 AS build
WORKDIR /src
COPY BTogether.sln ./
COPY ./BTogether.Web/*.csproj ./BTogether.Web/
COPY ./BTogether.Models/*.csproj ./BTogether.Models/
COPY ./BTogether.BussinessLayer/*.csproj ./BTogether.BussinessLayer/
COPY ./BTogether.Data/*.csproj ./BTogether.Data/
RUN dotnet restore ./BTogether.Web/*.csproj
COPY . .
RUN dotnet publish ./BTogether.Web/*.csproj -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.17-amd64 
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000
ENTRYPOINT ["dotnet","BTogether.Web.dll"]
