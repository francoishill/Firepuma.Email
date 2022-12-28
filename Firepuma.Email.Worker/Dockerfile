FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS restore
WORKDIR /src

COPY ["Firepuma.Email.Domain/Firepuma.Email.Domain.csproj", "Firepuma.Email.Domain/"]
COPY ["Firepuma.Email.Infrastructure/Firepuma.Email.Infrastructure.csproj", "Firepuma.Email.Infrastructure/"]
COPY ["Firepuma.Email.Tests/Firepuma.Email.Tests.csproj", "Firepuma.Email.Tests/"]

COPY ["Firepuma.Email.Worker/Firepuma.Email.Worker.csproj", "Firepuma.Email.Worker/"]
RUN dotnet restore "Firepuma.Email.Worker/Firepuma.Email.Worker.csproj"
COPY . .


FROM restore AS test
RUN dotnet restore "Firepuma.Email.Tests/Firepuma.Email.Tests.csproj"
RUN dotnet test --no-restore --logger:trx --results-directory /testsresults


FROM test AS publish
ARG target_configuration=Release
WORKDIR /src/Firepuma.Email.Worker
RUN dotnet publish  "Firepuma.Email.Worker.csproj" -c ${target_configuration} --no-restore -o /app


FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final
LABEL maintainer="Francois Hill <francoishill11@gmail.com>"
WORKDIR /app
COPY --from=publish /app .

ARG BUILD_ID
ENV BUILD_ID="${BUILD_ID}"

ENTRYPOINT ["dotnet", "Firepuma.Email.Worker.dll"]