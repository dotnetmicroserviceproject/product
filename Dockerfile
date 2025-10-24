FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5096

ENV ASPNETCORE_URLS=http://+:5096

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser


# -----------------------------
# Build stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["src/Product.Contracts/Product.Contracts.csproj", "src/Product.Contracts/"]
COPY ["src/Product.Service/Product.Service.csproj", "src/Product.Service/"]

# Authenticate to GitHub Packages using secrets and restore
RUN --mount=type=secret,id=GH_OWNER,dst=/GH_OWNER \
    --mount=type=secret,id=GH_PAT,dst=/GH_PAT \
    bash -c '\
        GH_OWNER=$(cat /GH_OWNER) && \
        GH_PAT=$(cat /GH_PAT) && \
        echo "machine nuget.pkg.github.com login ${GH_OWNER} password ${GH_PAT}" > ~/.netrc && \
        dotnet nuget add source \
            --username ${GH_OWNER} \
            --password ${GH_PAT} \
            --store-password-in-clear-text \
            --name github "https://nuget.pkg.github.com/${GH_OWNER}/index.json" && \
        dotnet restore "src/Product.Service/Product.Service.csproj" --disable-parallel --ignore-failed-sources --verbosity minimal \
    '

# Copy all source files
COPY ./src ./src

# ✅ Fix: Adjusted working directory to match your actual folder structure inside container
WORKDIR "/src/src/Product.Service"

# Publish the application
RUN dotnet publish "Product.Service.csproj" -c Release --no-restore -o /app/publish


# -----------------------------
# Final runtime image
# -----------------------------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Product.Service.dll"]