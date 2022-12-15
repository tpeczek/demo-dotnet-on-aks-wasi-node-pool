FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY . .
RUN dotnet build -c Release

FROM scratch
COPY --from=build /src/bin/Release/net7.0/Demo.Wasm.Spin.wasm ./bin/Release/net7.0/Demo.Wasm.Spin.wasm
COPY --from=build /src/spin.toml .