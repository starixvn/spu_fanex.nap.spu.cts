FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src

COPY ["./src/Fanex.NAP.SPU.CTS", "Fanex.NAP.SPU.CTS/"]

COPY ["./src/Fanex.NAP.SPU.CTS.JobService/Fanex.NAP.SPU.CTS.JobService.csproj", "Fanex.NAP.SPU.CTS.JobService/"]

WORKDIR "/src/Fanex.NAP.SPU.CTS.JobService"

RUN dotnet nuget add source http://nuget.nexdev.net/api/v2

RUN dotnet restore -v n

COPY ./src/Fanex.NAP.SPU.CTS.JobService .

RUN dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

WORKDIR /app

COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.NAP.SPU.CTS.JobService.dll"]

ENV TZ="America/La_Paz"