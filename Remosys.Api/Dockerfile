FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Remosys.Api/Remosys.Api.csproj", "Remosys.Api/"]
COPY ["Remosys.Common/Remosys.Common.csproj", "Remosys.Common/"]
RUN dotnet restore "Remosys.Api/Remosys.Api.csproj"
COPY . .
WORKDIR "/src/Remosys.Api"
RUN dotnet build "Remosys.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Remosys.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Remosys.Api.dll"]