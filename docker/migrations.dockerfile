FROM buildcontainer:latest AS build
# Copy everything else and build
WORKDIR /app/src/PizzaIllico.SqlMigrations
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/src/PizzaIllico.SqlMigrations/out .
COPY docker/wait-for-it.sh . 
COPY docker/wait-and-run.sh .
ENTRYPOINT ["./wait-and-run.sh", "dotnet", "PizzaIllico.SqlMigrations.dll"]
